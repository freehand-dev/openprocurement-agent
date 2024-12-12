using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using openprocurement_agent.Models;
using openprocurement_agent.Services;

namespace openprocurement_agent.MessagePipeline
{
    public class StatusFilter
    {
        static public TransformBlock<MessageTender, MessageTender> Create(
            TransformSettings_Status settings,
            ILogger<OpenprocurementService> logger)
        {
            return new TransformBlock<MessageTender, MessageTender>(message =>
            {
                if (!settings.Enabled)
                    return message;

                try
                {    
                    var allow = settings.Allow.Any(f => f.ToLower() == message.Item.Status.ToLower());
                    message.Status = allow ? MessageTenderStatus.NextTarget : MessageTenderStatus.NullTarget;
                }
                catch (Exception e)
                {
                    logger.LogError($"StatusFilter error with messages { e.Message }");
                }
                return message;     
            });
        }

    }
}
