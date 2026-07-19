using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using openprocurement_agent.Models;
using openprocurement_agent.Services;

namespace openprocurement_agent.MessagePipeline
{
    public class StatusFilter
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
                    StatusTransformSettings? settings;
                    lock (dbLock)
                    {
                        settings = pipelineSettingsDbContext.StatusTransformSettings.Find(1);
                    }
                    if (settings == null || !settings.Enabled)
                        return message;

                    var allowed = PipelineSettingsDbContext.ParseList(settings.Allow);
                    var allow = allowed.Any(f => f.ToLower() == message.Item!.Status.ToLower());
                    message.Status = allow ? MessageTenderStatus.NextTarget : MessageTenderStatus.NullTarget;
                }
                catch (Exception e)
                {
                    logger.LogError($"StatusFilter error with messages {e.Message}");
                }
                return message;
            });
        }

    }
}
