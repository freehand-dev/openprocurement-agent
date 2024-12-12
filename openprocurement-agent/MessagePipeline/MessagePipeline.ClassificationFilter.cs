using openprocurement_agent.Models;
using openprocurement_agent.Services;
using System.Threading.Tasks.Dataflow;

namespace openprocurement_agent.MessagePipeline
{
    public class ClassificationFilter
    {
        static public TransformBlock<MessageTender, MessageTender> Create(
            TransformSettings_Classification settings,
            ILogger<OpenprocurementService> logger)
        {
            return new TransformBlock<MessageTender, MessageTender>(message =>
            {
                if (!settings.Enabled)
                    return message;

                try
                {
                    bool isBypass = message.Item.Items.Any(item => settings.Bypass.Contains(item.Classification.Id));
                    bool isBlock = message.Item.Items.Any(item => settings.Block.Contains(item.Classification.Id));
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
                    logger.LogError($"ClassificationFilter error with messages { e.Message }");
                }

                return message;
            });
        }
    }
}
