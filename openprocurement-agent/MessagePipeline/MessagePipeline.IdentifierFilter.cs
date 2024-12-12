using openprocurement_agent.Models;
using openprocurement_agent.Services;
using System.Threading.Tasks.Dataflow;

namespace openprocurement_agent.MessagePipeline
{
    public class IdentifierFilter
    {
        static public TransformBlock<MessageTender, MessageTender> Create(
            TransformSettings_Identifier settings,
            Models.ProcuringEntityDbContex databaseContex,
            Object dbLock,
            ILogger<OpenprocurementService> logger)
        {
            return new TransformBlock<MessageTender, MessageTender>(message =>
            {
                if (!settings.Enabled)
                    return message;

                try
                {
                    lock (dbLock)
                    {
                        bool isMatch = databaseContex.ProcuringEntitys.Any(b => b.Code == message.Item.ProcuringEntity.Identifier.Id);
                        message.Status = isMatch ? MessageTenderStatus.NextTarget : MessageTenderStatus.NullTarget;
                    }
                }
                catch (Exception e)
                {
                    logger.LogError($"IdentifierFilter error with messages { e.Message }");
                }

                return message;
            });
        }
    }
}
