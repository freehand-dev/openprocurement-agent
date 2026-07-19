using openprocurement_agent.Models;
using openprocurement_agent.Services;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace openprocurement_agent.MessagePipeline
{
    public class TenderHistoryAction
    {
        static public ActionBlock<MessageTender> Create(
            Models.PipelineSettingsDbContext pipelineSettingsDbContext,
            Models.TenderHistoryDbContext? databaseContext,
            Object dbLock,
            ILogger<OpenprocurementService> logger)
        {
            return new ActionBlock<MessageTender>(delegate (MessageTender message)
            {
                try
                {
                    lock (dbLock)
                    {
                        var settings = pipelineSettingsDbContext.TendersHistoryActionSettings.Find(1);
                        if (settings == null || !settings.Enabled || databaseContext == null)
                            return;

                        databaseContext.Add(new TenderHistory { TenderId = message.Item!.TenderID, Title = message.Item!.Title, CreatedDate = DateTime.UtcNow });
                        databaseContext.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    logger.LogError($"TenderHistoryAction error with messages {e.Message}");
                }
            });
        }
    }
}
