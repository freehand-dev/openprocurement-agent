using Microsoft.Extensions.Logging;
using openprocurement.api.client.Models;
using openprocurement_agent.Models;
using openprocurement_agent.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace openprocurement_agent.MessagePipeline
{
    public class IdentifierFilter
    {
        static public TransformBlock<Tender, Tender> Create(
            TransformSettings_Identifier settings,
            Models.ProcuringEntityDbContex databaseContex,
            Object dbLock,
            ILogger<OpenprocurementService> logger)
        {
            return new TransformBlock<Tender, Tender>(message =>
            {
                try
                {
                    if (!settings.Enabled)
                        return message;

                    lock (dbLock)
                    {
                        bool findInHistory = databaseContex.ProcuringEntitys.Any(b => b.Code == message.ProcuringEntity.Identifier.Id);
                        return (findInHistory ? message : null);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError($"IdentifierFilter - { e.Message }");
                }

                return message;
            });
        }
    }
}
