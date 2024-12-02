using openprocurement.api.client;
using openprocurement.api.client.Exceptions;
using openprocurement.api.client.Models;
using openprocurement_agent;
using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestProject
{
    public class UnitTest1
    {
        private static readonly OpenprocurementClient client = new OpenprocurementClient();

        [Fact]
        public async Task Test1()
        {
            var x = await client.GetTendersAsync(DateTime.Now.AddDays(-1), 0);
            Debug.WriteLine($"{ x.NextPage.Offset }");
            // var x = await client.GetTendersAsync(DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)), 0);
        }

        [Fact]
        public async Task TestError()
        {
            try
            {
                var x = await client.GetTenderAsync("98a4044accb640738e805a0bfe245034;");
            }
            catch (ErrorResponseException e)
            {
                Debug.WriteLine($"{ e.Message }");
            }        
        }

        [Fact]
        public async Task TestTender()
        {
            var x = await client.GetTenderAsync("b6c1b8c0c2074bc8b9380cff823ee8e3");
            // Debug.WriteLine(
            //    JsonEnumConverter<Tender.StatusEnum>.GetEnumMemberValue(x.Data?.Status));
        }

        [Fact]
        public async Task TestStringTemplate001()
        {
            var x = await client.GetTenderAsync("b6c1b8c0c2074bc8b9380cff823ee8e3");
            var result = StringTemplate.ToString(@"%Value.String% - %Title% - (%ProcuringEntity.Name%)", x.Data);
            Assert.Equal(
                "6300000 UAH - 34520000-8 Човни (Моторний човен промірний) - (ФІЛІЯ \"ДНОПОГЛИБЛЮВАЛЬНИЙ ФЛОТ\" ДЕРЖАВНОГО ПІДПРИЄМСТВА \"АДМІНІСТРАЦІЯ МОРСЬКИХ ПОРТІВ УКРАЇНИ\")",
                result);
        }

        [Fact]
        public async Task TestToHTML001()
        {
            var x = await client.GetTenderAsync("b6c1b8c0c2074bc8b9380cff823ee8e3");

            System.IO.File.WriteAllText("tender.html", 
                x.Data.ToHTML().ToString());
        }

        [Fact]
        public async Task TestToHTML002()
        {
            var x = await client.GetTenderAsync("b6c1b8c0c2074bc8b9380cff823ee8e3");
            var message = x.Data;

            var body = System.IO.File.ReadAllText("message.html");
            if (!String.IsNullOrEmpty(body))
            {
                body = body.Replace("%body%", message.ToHTMLBody().ToString());
                body = StringTemplate.ToString(body, message);
            }

            System.IO.File.WriteAllText("tender.html",
                body);
        }


        [Fact]
        public async Task TestSendMail001()
        {
            var x = await client.GetTenderAsync("b6c1b8c0c2074bc8b9380cff823ee8e3");
            var message = x.Data;

            var From = "";
            var Username = "";
            var Password = "";
            var Server = "";
            var Port = 25;
            var EnableSsl = false;
            var Subject = "%Value.String% - %Title% - (%ProcuringEntity.Name%)";
            var MailTo = "";
            var MessageTemplateFile = "";


            // send mail
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage
            {
                From = new MailAddress(From),
                Subject = StringTemplate.ToString(Subject, message).Replace('\r', ' ').Replace('\n', ' '),
                IsBodyHtml = true,
            };

            mailMessage.To.Add(MailTo);

            string body = message.ToHTML().ToString();

            if (!String.IsNullOrWhiteSpace(MessageTemplateFile))
            {
                if (System.IO.File.Exists(MessageTemplateFile))
                {
                    body = System.IO.File.ReadAllText(MessageTemplateFile);
                    if (!String.IsNullOrWhiteSpace(body))
                    {
                        body = body.Replace("%body%", message.ToHTMLBody().ToString());
                        body = StringTemplate.ToString(body, message);
                    }
                }
            }

            mailMessage.Body = body;

            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(Server))
            {
                client.Credentials = new System.Net.NetworkCredential(Username, Password);
                client.Port = Port;
                client.EnableSsl = EnableSsl;
                client.Send(mailMessage);
            }
        }

        [Fact]
        public async Task TestTenderDocuments()
        {
            var x = await client.GetTenderDocumentsAsync("62a722e0afcb42eea8dd2c57f8c868f4");
        }
    }
}
