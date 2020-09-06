using Microsoft.Extensions.Logging;
using openprocurement.api.client.Models;
using openprocurement_agent.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace openprocurement_agent.MessagePipeline
{
    public class TenderHistoryFilter
    {
        static public TransformBlock<Tender, Tender>Create(
            Models.TransformSettings_TendersHistory settings,
            Models.TenderHistoryDbContex databaseContex,
            Object dbLock,
            ILogger<OpenprocurementService> logger)
        {
            return new TransformBlock<Tender, Tender>(message =>
            {
                try
                {
                    if (!settings.Enabled)
                        return message;

                    bool findInHistory = databaseContex.TenderHistory.Any(b => b.TenderId == message.TenderID);
                    return (findInHistory ? null : message);
                } 
                catch(Exception e)
                {
                    logger.LogError($"TenderHistoryFilter - { e.Message }");
                }

                return message;
            });
        }
    }
}
