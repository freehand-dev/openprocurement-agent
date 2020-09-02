using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.Logging;
using openprocurement.api.client.Models;
using openprocurement_agent.Models;
using openprocurement_agent.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks.Dataflow;

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
                    // send mail
                    ExchangeAction.SendMail(settings.Username, settings.Password, settings.MailTo, settings.ServerUrl, message);

                    logger.LogInformation($"!!!!!!!!!!!!!!!!!!![{ message.Id }][{message.DateModified:o}][{ message.Status }] { message.Title }");
                }
                catch (Exception e)
                {
                    logger.LogError($"TenderHistoryFilter - { e.Message }");
                }
                
            });
        }

        static public void SendMail(string MailUser, string MailPass, List<string> MailsTo, Uri ServerUrl, Tender tender)
        {
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
            service.Credentials = new NetworkCredential(MailUser, MailPass);
            service.Url = ServerUrl;
            EmailMessage emailMessage = new EmailMessage(service);
            emailMessage.Subject = $"{ tender.Title } - ({ tender.ProcuringEntity?.Name })";

            emailMessage.Body = new MessageBody(tender.ToHTML().ToString());
            foreach (string  mailTo in MailsTo)
                emailMessage.ToRecipients.Add(mailTo);
            emailMessage.Send();
        }


    }
}
