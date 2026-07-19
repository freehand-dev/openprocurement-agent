using openprocurement.api.client;
using openprocurement.api.client.Models;
using openprocurement_agent.MessagePipeline;
using openprocurement_agent.Models;

namespace openprocurement_agent.Services
{
    public class OpenprocurementService : BackgroundService, IDisposable
    {
        private readonly IOpenprocurementClient _client;
        private readonly ILogger<OpenprocurementService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly Models.TenderHistoryDbContext _tenderHistoryDbContext;
        private readonly Models.ProcuringEntityDbContext _procuringEntityDbContext;
        private readonly Models.PipelineSettingsDbContext _pipelineSettingsDbContext;

        private IMessagePipeline _pipeline;

        public OpenprocurementService(
            IOpenprocurementClient client,
            IServiceProvider serviceProvider,
            ILogger<OpenprocurementService> logger)
        {
            this._client = client;
            this._logger = logger;
            this._serviceProvider = serviceProvider;

            this._procuringEntityDbContext = (Models.ProcuringEntityDbContext)_serviceProvider.CreateScope().ServiceProvider.GetRequiredService(typeof(Models.ProcuringEntityDbContext));
            this._tenderHistoryDbContext = (Models.TenderHistoryDbContext)_serviceProvider.CreateScope().ServiceProvider.GetRequiredService(typeof(Models.TenderHistoryDbContext));
            this._pipelineSettingsDbContext = (Models.PipelineSettingsDbContext)_serviceProvider.CreateScope().ServiceProvider.GetRequiredService(typeof(Models.PipelineSettingsDbContext));

            // Seed pipeline (incl. mail action) settings defaults on first run
            this._pipelineSettingsDbContext.SeedDefaults();

            // build message pipeline
            _pipeline = new MessagePipeline.MessagePipeline(this._tenderHistoryDbContext, this._procuringEntityDbContext, this._pipelineSettingsDbContext, this._logger);
        }

        public override void Dispose()
        {
            _tenderHistoryDbContext?.Dispose();

            _procuringEntityDbContext?.Dispose();
        }

        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Openprocurement Service started at: {DateTimeOffset.UtcNow.ToString("o")}");
            await base.StartAsync(stoppingToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                int subtractHours = this._pipelineSettingsDbContext.GlobalSettings.Find(1)?.Subtract ?? 1;

                // Initial request to obtain tender data
                TendersResponse response = await _client.GetTendersAsync(
                    DateTimeOffset.UtcNow.Subtract(TimeSpan.FromHours(subtractHours)), 100, stoppingToken);

                do
                {
                    try
                    {
                        _logger.LogInformation($"Retrieve tenders list from offset: {response.PrevPage.Offset}");

                        await ProcessTendersAsync(response, stoppingToken);

                        // Checking if there is data and if there is a next page
                        if (response.Data == null || response.Data.Count == 0)
                        {
                            _logger.LogInformation("No tenders found, pausing for 5 minutes.");
                            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                        }

                        // Getting data from the next page
                        response = await _client.GetTendersAsync(response.NextPage, stoppingToken);
                    }
                    catch (TaskCanceledException)
                    { }
                    catch (Exception ex)
                    {
                        _logger.LogError($"An error occurred while processing tenders: {ex.Message}");
                    }
                }
                while (!stoppingToken.IsCancellationRequested);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to start processing tenders: {ex.Message}");
            }
        }

        private async Task ProcessTendersAsync(TendersResponse response, CancellationToken stoppingToken)
        {
            foreach (TenderBase tenderBase in response.Data)
            {
                if (stoppingToken.IsCancellationRequested)
                    break;

                try
                {
                    var x = await _client.GetTenderAsync(tenderBase.Id, stoppingToken);
                    Tender tender = x.Data;
                    _logger.LogDebug($"[{tender.Id}][{tender.DateModified.ToString("o")}][{tender.Status}] {tender.Title}");

                    await _pipeline.SendAsync(tender);
                }
                catch (Exception e)
                {
                    _logger.LogError($"[{tenderBase.Id}][{tenderBase.DateModified.ToString("o")}] {e.Message}");
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
