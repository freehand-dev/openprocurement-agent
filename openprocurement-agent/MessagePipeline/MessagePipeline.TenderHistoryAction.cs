using openprocurement_agent.Models;
using openprocurement_agent.Services;
using System.Threading.Tasks.Dataflow;

namespace openprocurement_agent.MessagePipeline
{
    public class TenderHistoryAction
    {
        static public ActionBlock<MessageTender> Create(
            Models.ActionSetting_TendersHistory settings,
            Models.TenderHistoryDbContex databaseContex,
            Object dbLock,
            ILogger<OpenprocurementService> logger)
        {
            return new ActionBlock<MessageTender>(delegate (MessageTender message)
            {
                if (!settings.Enabled)
                    return;

                try
                {
                    databaseContex.Add(new TenderHistory { TenderId = message.Item.TenderID });
                    databaseContex.SaveChanges();
                }
                catch (Exception e)
                {
                    logger.LogError($"TenderHistoryAction error with messages { e.Message }");
                }
            });
        }
    }
}
