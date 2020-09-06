using Microsoft.Extensions.Logging;
using openprocurement.api.client.Models;
using openprocurement_agent.MessagePipeline;
using openprocurement_agent.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace openprocurement_agent.MessagePipeline
{
    public interface IMessagePipeline
    {
        public Task<bool> SendAsync(Tender item);
    }

    public class MessagePipeline: IMessagePipeline, IDisposable
    {

        Object _dbLock = new Object();

        private DataflowLinkOptions linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

        public BufferBlock<Tender> BufferBlock { get; } = new BufferBlock<Tender>();

        public MessagePipeline(
            Models.AppSettings settings,
            Models.TenderHistoryDbContex tenderHistoryDbContex,
            Models.ProcuringEntityDbContex procuringEntityDbContex,
            ILogger<OpenprocurementService> logger)
        {        
            // ExchangeAction
            var exchangeAction = ExchangeAction.Create(settings.Action.SendMail, logger);

            // TenderHistoryAction
            var tenderHistoryAction = TenderHistoryAction.Create(settings.Action.TendersHistory, tenderHistoryDbContex, _dbLock, logger);

            var broadcastBlock = new BroadcastBlock<Tender>(msg => msg);

            // StatusFilter
            var statusFilter = StatusFilter.Create(settings.Transform.Status, logger);

            // TenderHistoryFilter
            var tenderHistoryFilter = TenderHistoryFilter.Create(settings.Transform.TendersHistory, tenderHistoryDbContex, _dbLock, logger);

            // IdentifierFilter
            var identifierFilter = IdentifierFilter.Create(settings.Transform.Identifier, procuringEntityDbContex, _dbLock, logger);

            // BufferBlock - StatusFilter
            BufferBlock.LinkTo(statusFilter, linkOptions, message => message != null);
            BufferBlock.LinkTo(DataflowBlock.NullTarget<Tender>());
 
            // StatusFilter - TenderHistoryFilter
            statusFilter.LinkTo(identifierFilter, linkOptions, message => message != null);
            statusFilter.LinkTo(DataflowBlock.NullTarget<Tender>());

            // StatusFilter - IdentifierFilter
            identifierFilter.LinkTo(tenderHistoryFilter, linkOptions, message => message != null);
            identifierFilter.LinkTo(DataflowBlock.NullTarget<Tender>());
            
            // TenderHistoryFilter - BroadcastBlock
            tenderHistoryFilter.LinkTo(broadcastBlock, linkOptions, message => message != null);
            tenderHistoryFilter.LinkTo(DataflowBlock.NullTarget<Tender>());

            // BroadcastBlock - ExchangeAction
            broadcastBlock.LinkTo(exchangeAction, linkOptions, message => message != null);

            // BroadcastBlock - TenderHistoryAction
            broadcastBlock.LinkTo(tenderHistoryAction, linkOptions);

        }



        public Task<bool> SendAsync(Tender item)
        {
            return this.BufferBlock.SendAsync(item);
        }

        public void Dispose()
        {
            
        }
    }
}
