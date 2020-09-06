using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using openprocurement.api.client.Models;
using openprocurement_agent.Models;
using openprocurement_agent.Services;

namespace openprocurement_agent.MessagePipeline
{
    public class StatusFilter
    {
        static public TransformBlock<Tender, Tender> Create(
            TransformSettings_Status settings,
            ILogger<OpenprocurementService> logger)
        {
            return new TransformBlock<Tender, Tender>(message =>
            {
                try
                {
                    if (!settings.Enabled)
                        return message;

                    var allow = settings.Allow.Any(f => f.ToLower() == message.Status.ToLower());
                    return allow ? message : null;
                }
                catch (Exception e)
                {
                    logger.LogError($"StatusFilter - { e.Message }");
                }
                return message;     
            });
        }

    }
}
