using System.Net.Mail;
using System.Threading.Tasks.Dataflow;
using openprocurement_agent.Models;
using openprocurement_agent.Services;

namespace openprocurement_agent.MessagePipeline
{
    public class ExchangeAction
    {
        static public ActionBlock<MessageTender> Create(
            PipelineSettingsDbContext pipelineDb,
            Object dbLock,
            ILogger<OpenprocurementService> logger)
        {
            return new ActionBlock<MessageTender>(delegate (MessageTender message)
            {

                if (message.Item == null)
                {
                    logger.LogWarning($"ExchangeAction: message.Item is null [{message.Status}]");
                    return;
                }

                try
                {
                    // Read mail settings from DB
                    MailSettings? mailSettings;
                    lock (dbLock)
                    {
                        mailSettings = pipelineDb.MailSettings.Find(1);
                    }
                    if (mailSettings == null || !mailSettings.Enabled)
                        return;

                    // Parse MailTo (one per line)
                    var recipients = mailSettings.MailTo
                        .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList();

                    if (recipients.Count == 0)
                    {
                        logger.LogWarning("ExchangeAction: no recipients configured");
                        return;
                    }

                    // send mail
                    System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                    mailMessage.From = new MailAddress(mailSettings.From);
                    foreach (string mailTo in recipients)
                        mailMessage.To.Add(mailTo);
                    mailMessage.Subject = StringTemplate.ToString(mailSettings.Subject, message.Item).Replace('\r', ' ').Replace('\n', ' ');
                    mailMessage.IsBodyHtml = true;
                    string body = message.Item.ToHtml().ToString();

                    if (!String.IsNullOrWhiteSpace(mailSettings.MessageTemplate))
                    {
                        body = mailSettings.MessageTemplate;
                        body = body.Replace("%body%", message.Item.ToHtmlBody().ToString());
                        body = StringTemplate.ToString(body, message.Item);
                    }

                    mailMessage.Body = body;

                    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(mailSettings.Server))
                    {
                        client.Credentials = new System.Net.NetworkCredential(mailSettings.Username, mailSettings.Password);
                        client.Port = mailSettings.Port;
                        client.EnableSsl = mailSettings.EnableSsl;
                        client.Send(mailMessage);
                    }


                    logger.LogInformation($"Information about a Tender sent successfully [{message.Item.Id}][{message.Item.DateModified:o}][{message.Status}] {message.Item.Title}");
                }
                catch (Exception e)
                {
                    logger.LogError($"ExchangeAction error with messages {e.Message}");
                }

            });
        }




    }
}
