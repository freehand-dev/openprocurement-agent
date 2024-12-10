using Microsoft.Extensions.DependencyInjection;
using openprocurement.api.client;
using openprocurement.api.client.Exceptions;
using openprocurement.api.client.Models;
using openprocurement_agent;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestProject
{
    public class UnitTest1
    {

        [Fact]
        public async Task GetTendersAsyncTest()
        {
            var services = new ServiceCollection();
            services.AddHttpClient<IOpenprocurementClient, OpenprocurementClient>();
            var serviceProvider = services.BuildServiceProvider();
            var client = serviceProvider.GetRequiredService<IOpenprocurementClient>();

            var x = await client.GetTendersAsync(DateTimeOffset.Now.AddDays(-1), 50, CancellationToken.None);

            Assert.NotNull(x);
            Assert.NotNull(x.Data);
            Assert.Equal(50, x.Data.Count);
        }

        [Fact]
        public async Task GetTenderAsyncExceptionTest()
        {
            var services = new ServiceCollection();
            services.AddHttpClient<IOpenprocurementClient, OpenprocurementClient>();
            var serviceProvider = services.BuildServiceProvider();
            var client = serviceProvider.GetRequiredService<IOpenprocurementClient>();

            try
            {
                var x = await client.GetTenderAsync("98a4044accb640738e805a0bfe245034;", CancellationToken.None);
                Assert.Fail();
            }
            catch (ErrorResponseException e)
            {
                Debug.WriteLine($"{ e.Message }");
            }        
        }

        [Fact]
        public async Task GetTenderAsyncTest()
        {
            var services = new ServiceCollection();
            services.AddHttpClient<IOpenprocurementClient, OpenprocurementClient>();
            var serviceProvider = services.BuildServiceProvider();
            var client = serviceProvider.GetRequiredService<IOpenprocurementClient>();

            var x = await client.GetTenderAsync("b6c1b8c0c2074bc8b9380cff823ee8e3", CancellationToken.None);
            Assert.NotNull(x);
            Assert.NotNull(x.Data);
            Assert.Equal("UA-2020-08-28-005435-c", x.Data.TenderID);
            Assert.Single(x.Data.Items);    
        }

        [Fact]
        public async Task StringTemplateTest()
        {
            var services = new ServiceCollection();
            services.AddHttpClient<IOpenprocurementClient, OpenprocurementClient>();
            var serviceProvider = services.BuildServiceProvider();
            var client = serviceProvider.GetRequiredService<IOpenprocurementClient>();

            var x = await client.GetTenderAsync("b6c1b8c0c2074bc8b9380cff823ee8e3", CancellationToken.None);
            var result = StringTemplate.ToString(@"%Value.String% - %Title% - (%ProcuringEntity.Name%)", x.Data);
            Assert.Equal(
                "6300000 UAH - 34520000-8 Човни (Моторний човен промірний) - (ФІЛІЯ \"ДНОПОГЛИБЛЮВАЛЬНИЙ ФЛОТ\" ДЕРЖАВНОГО ПІДПРИЄМСТВА \"АДМІНІСТРАЦІЯ МОРСЬКИХ ПОРТІВ УКРАЇНИ\")",
                result);
        }

        [Fact]
        public async Task TenderToHTMLTest()
        {
            var services = new ServiceCollection();
            services.AddHttpClient<IOpenprocurementClient, OpenprocurementClient>();
            var serviceProvider = services.BuildServiceProvider();
            var client = serviceProvider.GetRequiredService<IOpenprocurementClient>();

            var x = await client.GetTenderAsync("b6c1b8c0c2074bc8b9380cff823ee8e3", CancellationToken.None);

            var html = x.Data.ToHTML();
        }

        [Fact]
        public async Task TenderToHTMLTemplateTest()
        {
            var services = new ServiceCollection();
            services.AddHttpClient<IOpenprocurementClient, OpenprocurementClient>();
            var serviceProvider = services.BuildServiceProvider();
            var client = serviceProvider.GetRequiredService<IOpenprocurementClient>();

            var x = await client.GetTenderAsync("b6c1b8c0c2074bc8b9380cff823ee8e3", CancellationToken.None);
            var message = x.Data;

            var body = System.IO.File.ReadAllText("message.html");
            if (!String.IsNullOrEmpty(body))
            {
                body = body.Replace("%body%", message.ToHTMLBody().ToString());
                body = StringTemplate.ToString(body, message);
            }
        }


        [Fact]
        public async Task TestSendMail001()
        {
            var services = new ServiceCollection();
            services.AddHttpClient<IOpenprocurementClient, OpenprocurementClient>();
            var serviceProvider = services.BuildServiceProvider();
            var client = serviceProvider.GetRequiredService<IOpenprocurementClient>();

            var x = await client.GetTenderAsync("b6c1b8c0c2074bc8b9380cff823ee8e3", CancellationToken.None);
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

            using (System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient(Server))
            {
                smtpClient.Credentials = new System.Net.NetworkCredential(Username, Password);
                smtpClient.Port = Port;
                smtpClient.EnableSsl = EnableSsl;
                smtpClient.Send(mailMessage);
            }
        }

        [Fact]
        public async Task GetTenderDocumentsAsyncTest()
        {
            var services = new ServiceCollection();
            services.AddHttpClient<IOpenprocurementClient, OpenprocurementClient>();
            var serviceProvider = services.BuildServiceProvider();
            var client = serviceProvider.GetRequiredService<IOpenprocurementClient>();

            var x = await client.GetTenderDocumentsAsync("62a722e0afcb42eea8dd2c57f8c868f4", CancellationToken.None);

            Assert.NotNull(x);
        }
    }
}
