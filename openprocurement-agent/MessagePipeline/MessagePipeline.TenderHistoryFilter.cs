using openprocurement_agent.Services;
using System.Threading.Tasks.Dataflow;

namespace openprocurement_agent.MessagePipeline
{
    public class TenderHistoryFilter
    {
        static public TransformBlock<MessageTender, MessageTender> Create(
            Models.TransformSettings_TendersHistory settings,
            Models.TenderHistoryDbContex databaseContex,
            Object dbLock,
            ILogger<OpenprocurementService> logger)
        {
            return new TransformBlock<MessageTender, MessageTender>(message =>
            {
                if (!settings.Enabled)
                    return message;

                try
                {
                    bool isMatch = databaseContex.TenderHistory.Any(b => b.TenderId == message.Item.TenderID);
                    message.Status = isMatch ? MessageTenderStatus.NullTarget : MessageTenderStatus.NextTarget;
                } 
                catch(Exception e)
                {
                    logger.LogError($"TenderHistoryFilter error with messages { e.Message }");
                }

                return message;
            });
        }
    }
}
