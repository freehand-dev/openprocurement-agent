using openprocurement_agent.Models;
using openprocurement_agent.Services;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace openprocurement_agent.MessagePipeline
{
    public class IdentifierFilter
    {
        static public TransformBlock<MessageTender, MessageTender> Create(
            PipelineSettingsDbContext pipelineSettingsDbContext,
            Models.ProcuringEntityDbContext? databaseContex,
            Object dbLock,
            ILogger<OpenprocurementService> logger)
        {
            return new TransformBlock<MessageTender, MessageTender>(message =>
            {
                try
                {
                    IdentifierTransformSettings? settings;
                    bool isMatch;
                    lock (dbLock)
                    {
                        settings = pipelineSettingsDbContext.IdentifierTransformSettings.Find(1);
                        if (settings == null || !settings.Enabled || databaseContex == null)
                            return message;

                        isMatch = databaseContex.ProcuringEntitys.Any(b => b.Code == message.Item!.ProcuringEntity.Identifier.Id);
                    }
                    message.Status = isMatch ? MessageTenderStatus.NextTarget : MessageTenderStatus.NullTarget;
                }
                catch (Exception e)
                {
                    logger.LogError($"IdentifierFilter error with messages {e.Message}");
                }

                return message;
            });
        }
    }
}
