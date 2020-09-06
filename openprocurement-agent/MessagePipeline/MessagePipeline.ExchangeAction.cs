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
                    ExchangeAction.SendMail(settings.Username, settings.Password, settings.MailTo, settings.Server, settings.Port, settings.EnableSsl, message);

                    logger.LogInformation($"Information about a Tender sent successfully [{ message.Id }][{message.DateModified:o}][{ message.Status }] { message.Title }");
                }
                catch (Exception e)
                {
                    logger.LogError($"ExchangeAction - { e.Message }");
                }
                
            });
        }

        static public void SendMail(string MailUser, string MailPass, List<string> MailsTo, string SmtpServer, int Port, bool EnableSsl, Tender tender)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
        
            message.From = new MailAddress("openprocurement@engineer-service.tv", "Tenders Agent"); 
            foreach (string mailTo in MailsTo)
                message.To.Add(mailTo);
            message.Subject = $"{ tender.Title } - ({ tender.ProcuringEntity?.Name })".Replace('\r', ' ').Replace('\n', ' ');
            message.IsBodyHtml = true;
            message.Body = tender.ToHTML().ToString();

            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(SmtpServer)) 
            {
                client.Credentials = new System.Net.NetworkCredential(MailUser, MailPass); 
                client.Port = Port; 
                client.EnableSsl = EnableSsl;
                client.Send(message);
            }
        }


    }
}
