using openprocurement_agent.Models;
using openprocurement_agent.Services;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace openprocurement_agent.MessagePipeline
{
    public class TenderHistoryFilter
    {
        static public TransformBlock<MessageTender, MessageTender> Create(
            Models.PipelineSettingsDbContext pipelineSettingsDbContext,
            Models.TenderHistoryDbContext? databaseContext,
            Object dbLock,
            ILogger<OpenprocurementService> logger)
        {
            return new TransformBlock<MessageTender, MessageTender>(message =>
            {
                try
                {
                    TendersHistoryTransformSettings? settings;
                    bool isMatch;
                    lock (dbLock)
                    {
                        settings = pipelineSettingsDbContext.TendersHistoryTransformSettings.Find(1);
                        if (settings == null || !settings.Enabled || databaseContext == null)
                            return message;

                        isMatch = databaseContext.TenderHistory.Any(b => b.TenderId == message.Item!.TenderID);
                    }
                    message.Status = isMatch ? MessageTenderStatus.NullTarget : MessageTenderStatus.NextTarget;
                }
                catch (Exception e)
                {
                    logger.LogError($"TenderHistoryFilter error with messages {e.Message}");
                }

                return message;
            });
        }
    }
}
