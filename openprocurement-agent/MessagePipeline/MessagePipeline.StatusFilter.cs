using Microsoft.Extensions.Logging;
using openprocurement.api.client.Models;
using openprocurement_agent.Services;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace openprocurement_agent.MessagePipeline
{
    public class StatusFilter
    {
        static public TransformBlock<Tender, Tender> Create(
            ILogger<OpenprocurementService> logger)
        {
            return new TransformBlock<Tender, Tender>(message =>
            {
                try
                {
                    // return message.Status.Contains("active") ? message : null;
                    return message;
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
