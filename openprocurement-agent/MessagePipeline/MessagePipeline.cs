using openprocurement.api.client.Models;
using openprocurement_agent.Services;
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

        public BufferBlock<MessageTender> BufferBlock { get; } = new BufferBlock<MessageTender>();

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

            var broadcastBlock = new BroadcastBlock<MessageTender>(msg => msg);

            // StatusFilter
            var statusFilter = StatusFilter.Create(settings.Transform.Status, logger);

            // TenderHistoryFilter
            var tenderHistoryFilter = TenderHistoryFilter.Create(settings.Transform.TendersHistory, tenderHistoryDbContex, _dbLock, logger);

            // IdentifierFilter
            var identifierFilter = IdentifierFilter.Create(settings.Transform.Identifier, procuringEntityDbContex, _dbLock, logger);

            // ClassificationFilter
            var classificationFilter = ClassificationFilter.Create(settings.Transform.Classification, logger);

            // BufferBlock - StatusFilter
            BufferBlock.LinkTo(statusFilter, linkOptions, message => message != null);
            BufferBlock.LinkTo(DataflowBlock.NullTarget<MessageTender>());


            // StatusFilter - ClassificationFilter
            statusFilter.LinkTo(classificationFilter, linkOptions, message => message.Status == MessageTenderStatus.NextTarget);
            statusFilter.LinkTo(DataflowBlock.NullTarget<MessageTender>(), linkOptions, message => message.Status == MessageTenderStatus.NullTarget);

            // ClassificationFilter - IdentifierFilter / TenderHistoryFilter
            classificationFilter.LinkTo(identifierFilter, linkOptions, message => message.Status == MessageTenderStatus.NextTarget);
            classificationFilter.LinkTo(tenderHistoryFilter, linkOptions, message => message.Status == MessageTenderStatus.SendTarget);
            classificationFilter.LinkTo(DataflowBlock.NullTarget<MessageTender>(), linkOptions, message => message.Status == MessageTenderStatus.NullTarget);

            // IdentifierFilter - TenderHistoryFilter
            identifierFilter.LinkTo(tenderHistoryFilter, linkOptions, message => message.Status != MessageTenderStatus.NullTarget);
            identifierFilter.LinkTo(DataflowBlock.NullTarget<MessageTender>(), linkOptions, message => message.Status == MessageTenderStatus.NullTarget);
            

            // TenderHistoryFilter - BroadcastBlock
            tenderHistoryFilter.LinkTo(broadcastBlock, linkOptions, message => message.Status != MessageTenderStatus.NullTarget);
            tenderHistoryFilter.LinkTo(DataflowBlock.NullTarget<MessageTender>(), linkOptions, message => message.Status == MessageTenderStatus.NullTarget);

            // BroadcastBlock - ExchangeAction
            broadcastBlock.LinkTo(exchangeAction, linkOptions, message => message != null);

            // BroadcastBlock - TenderHistoryAction
            broadcastBlock.LinkTo(tenderHistoryAction, linkOptions);

        }

        public Task<bool> SendAsync(Tender item)
        {
            return this.BufferBlock.SendAsync(
                new MessageTender() { Item = item });
        }

        public void Dispose()
        {
            
        }
    }
}
