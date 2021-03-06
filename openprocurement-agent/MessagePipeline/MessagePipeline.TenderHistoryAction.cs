﻿using Microsoft.Extensions.Logging;
using openprocurement.api.client.Models;
using openprocurement_agent.Models;
using openprocurement_agent.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace openprocurement_agent.MessagePipeline
{
    public class TenderHistoryAction
    {
        static public ActionBlock<Tender> Create(
            Models.ActionSetting_TendersHistory settings,
            Models.TenderHistoryDbContex databaseContex,
            Object dbLock,
            ILogger<OpenprocurementService> logger)
        {
            return new ActionBlock<Tender>(delegate (Tender message)
            {
                try
                {
                    if (!settings.Enabled)
                        return;

                    databaseContex.Add(new TenderHistory { TenderId = message.TenderID });
                    databaseContex.SaveChanges();
                }
                catch (Exception e)
                {
                    logger.LogError($"TenderHistoryAction - { e.Message }");
                }
            });
        }
    }
}
