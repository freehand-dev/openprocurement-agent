using openprocurement_agent.Models;
using openprocurement_agent.Services;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace openprocurement_agent.MessagePipeline
{
    public class ClassificationFilter
    {
        static public TransformBlock<MessageTender, MessageTender> Create(
            PipelineSettingsDbContext pipelineSettingsDbContext,
            Object dbLock,
            ILogger<OpenprocurementService> logger)
        {
            return new TransformBlock<MessageTender, MessageTender>(message =>
            {
                try
                {
                    ClassificationTransformSettings? settings;
                    lock (dbLock)
                    {
                        settings = pipelineSettingsDbContext.ClassificationTransformSettings.Find(1);
                    }
                    if (settings == null || !settings.Enabled)
                        return message;

                    var bypassList = PipelineSettingsDbContext.ParseList(settings.Bypass);
                    var blockList = PipelineSettingsDbContext.ParseList(settings.Block);

                    bool isBypass = message.Item!.Items.Any(item => bypassList.Contains(item.Classification.Id));
                    bool isBlock = message.Item!.Items.Any(item => blockList.Contains(item.Classification.Id));
                    if (isBypass)
                    {
                        message.Status = MessageTenderStatus.SendTarget;
                        logger.LogInformation($"Classification: Bypass Tender [{message.Item.Id}][{message.Item.DateModified:o}][{message.Status}] {message.Item.Title}");
                    }
                    else if (isBlock)
                    {
                        message.Status = MessageTenderStatus.NullTarget;
                        logger.LogInformation($"Classification: Block Tender [{message.Item.Id}][{message.Item.DateModified:o}][{message.Status}] {message.Item.Title}");
                    }
                    else
                    {
                        message.Status = MessageTenderStatus.NextTarget;
                    }
                }
                catch (Exception e)
                {
                    logger.LogError($"ClassificationFilter error with messages {e.Message}");
                }

                return message;
            });
        }
    }
}
