using System.Net.Mail;
using System.Threading.Tasks.Dataflow;
using openprocurement_agent.Models;
using openprocurement_agent.Services;

namespace openprocurement_agent.MessagePipeline
{
    public class ExchangeAction
    {
        static public ActionBlock<MessageTender> Create(
            ActionSetting_SendMail settings,
            ILogger<OpenprocurementService> logger)
        {
            return new ActionBlock<MessageTender>(delegate (MessageTender message)
            {
                if (!settings.Enabled)
                    return;

                try
                {
                    // send mail
                    System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                    mailMessage.From = new MailAddress(settings.From);
                    foreach (string mailTo in settings.MailTo)
                        mailMessage.To.Add(mailTo);
                    mailMessage.Subject = StringTemplate.ToString(settings.Subject, message).Replace('\r', ' ').Replace('\n', ' ');
                    mailMessage.IsBodyHtml = true;
                    string body = message.Item.ToHTML().ToString();

                    if (!String.IsNullOrWhiteSpace(settings.MessageTemplateFile))
                    {
                        if (System.IO.File.Exists(settings.MessageTemplateFile))
                        {
                            body = System.IO.File.ReadAllText(settings.MessageTemplateFile);
                            if (!String.IsNullOrWhiteSpace(body))
                            {
                                body = body.Replace("%body%", message.Item.ToHTMLBody().ToString());
                                body = StringTemplate.ToString(body, message);
                            }           
                        }
                    }

                    mailMessage.Body = body;

                    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(settings.Server))
                    {
                        client.Credentials = new System.Net.NetworkCredential(settings.Username, settings.Password);
                        client.Port = settings.Port;
                        client.EnableSsl = settings.EnableSsl;
                        client.Send(mailMessage);
                    }


                    logger.LogInformation($"Information about a Tender sent successfully [{ message.Item.Id }][{message.Item.DateModified:o}][{ message.Status }] { message.Item.Title }");
                }
                catch (Exception e)
                {
                    logger.LogError($"ExchangeAction error with messages { e.Message }");
                }
 
            });
        }




    }
}
