using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Logging;
using openprocurement.api.client.Models;
using openprocurement_agent.Models;
using openprocurement_agent.Services;

namespace openprocurement_agent.MessagePipeline
{
    public class ExchangeAction
    {
        static public ActionBlock<Tender> Create(
            ActionSetting_SendMail settings,
            ILogger<OpenprocurementService> logger)
        {
            return new ActionBlock<Tender>(delegate (Tender message)
            {
                try
                {
                    if (!settings.Enabled)
                        return;

                    // send mail
                    System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                    mailMessage.From = new MailAddress(settings.From);
                    foreach (string mailTo in settings.MailTo)
                        mailMessage.To.Add(mailTo);
                    mailMessage.Subject = StringTemplate.ToString(settings.Subject, message).Replace('\r', ' ').Replace('\n', ' ');
                    mailMessage.IsBodyHtml = true;
                    string body = message.ToHTML().ToString();

                    if (!String.IsNullOrEmpty(settings.MessageTemplateFile))
                    {
                        if (System.IO.File.Exists(settings.MessageTemplateFile))
                        {
                            body = System.IO.File.ReadAllText(settings.MessageTemplateFile);
                            if (!String.IsNullOrEmpty(body))
                                body = body.Replace("%body%", message.ToHTMLBody().ToString());
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


                    logger.LogInformation($"Information about a Tender sent successfully [{ message.Id }][{message.DateModified:o}][{ message.Status }] { message.Title }");
                }
                catch (Exception e)
                {
                    logger.LogError($"ExchangeAction - { e.Message }");
                }
                
            });
        }




    }
}
