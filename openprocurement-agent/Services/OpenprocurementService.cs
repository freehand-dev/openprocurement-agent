using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using openprocurement.api.client;
using openprocurement.api.client.Exceptions;
using openprocurement.api.client.Models;
using openprocurement_agent.MessagePipeline;
using openprocurement_agent.Models;

namespace openprocurement_agent.Services
{
    public class OpenprocurementService : BackgroundService, IDisposable
    {
        private readonly ILogger<OpenprocurementService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly AppSettings _settings;
        private readonly Models.TenderHistoryDbContex _tenderHistoryDbContex;
        private readonly Models.ProcuringEntityDbContex _procuringEntityDbContex;

        private IOpenprocurementClient _client = new OpenprocurementClient();

        private IMessagePipeline _pipeline;

        public OpenprocurementService(
            IServiceProvider serviceProvider,
            IOptions<AppSettings> settings, 
            ILogger<OpenprocurementService> logger)
        {
            this._logger = logger;
            this._serviceProvider = serviceProvider;
            this._settings = settings.Value;

            if (this._settings.Transform.Identifier.Enabled)
                this._tenderHistoryDbContex = (Models.TenderHistoryDbContex)_serviceProvider.CreateScope().ServiceProvider.GetRequiredService(typeof(Models.TenderHistoryDbContex));

            if (this._settings.Transform.TendersHistory.Enabled || this._settings.Action.TendersHistory.Enabled)
                this._procuringEntityDbContex = (Models.ProcuringEntityDbContex)_serviceProvider.CreateScope().ServiceProvider.GetRequiredService(typeof(Models.ProcuringEntityDbContex));

            // build message pipeline
            _pipeline = new MessagePipeline.MessagePipeline(this._settings, this._tenderHistoryDbContex, this._procuringEntityDbContex, this._logger);
        }

        public override void Dispose()
        {
            _tenderHistoryDbContex?.Dispose();

            _procuringEntityDbContex?.Dispose();
        }

        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Openprocurement Service started at: { DateTime.Now }");
            _logger.LogInformation($"Openprocurement Service settings: { JsonSerializer.Serialize(this._settings) }");


            await base.StartAsync(stoppingToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // startup offset, subtract from now Datetime
            DateTimeOffset offset = DateTime.Now.Subtract(TimeSpan.FromHours(this._settings.Global.Subtract));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //
                    _logger.LogInformation("Retrieve tenders list from {time}", offset.ToString("o"));

                    // get tenders
                    TendersResponse response = await _client.GetTendersAsync(offset);
                    foreach (TenderBase tenderBase in response.Data)
                    {
                        if (stoppingToken.IsCancellationRequested)
                            break;

                        try
                        {
                            var x = await _client.GetTenderAsync(tenderBase.Id);
                            Tender tender = x.Data;
                            _logger.LogDebug($"[{ tender.Id }][{tender.DateModified:o}][{ tender.Status }] { tender.Title }");

                            await _pipeline.SendAsync(tender);
                        }
                        catch (ErrorResponseException e)
                        {
                            _logger.LogError($"[{ tenderBase.Id }][{ tenderBase.DateModified.ToString("o") }] { e.Message }");
                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"[{ tenderBase.Id }][{ tenderBase.DateModified.ToString("o") }] { e.Message }");
                        }
                    }


                    // End of Tenders, wait for new tenders
                    if (response.NextPage.Offset == offset)
                    {
                        /// The safe frequency of synchronization requests is once per 5 minutes.
                        /// http://api-docs.openprocurement.org/en/latest/tenders.html
                        await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                    }

                    offset = response.NextPage.Offset;
                } 
                catch (TaskCanceledException)
                { }
                catch (Exception e)
                {
                    this._logger.LogError($"Internal Error: { e.Message } { e.InnerException?.Message }");
                }
               
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Openprocurement Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
