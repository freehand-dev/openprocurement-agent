using openprocurement.api.client;
using openprocurement.api.client.Exceptions;
using openprocurement.api.client.Models;
using openprocurement_agent;
using System;
using System.Diagnostics;
using Xunit;

namespace XUnitTestProject
{
    public class UnitTest1
    {
        private static readonly OpenprocurementClient client = new OpenprocurementClient();

        [Fact]
        public async void Test1()
        {
            var x = await client.GetTendersAsync(DateTime.Now.AddDays(1), 0);
            // var x = await client.GetTendersAsync(DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)), 0);
        }

        [Fact]
        public async void TestError()
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
        public async void TestTender()
        {
            var x = await client.GetTenderAsync("b6c1b8c0c2074bc8b9380cff823ee8e3");
            // Debug.WriteLine(
            //    JsonEnumConverter<Tender.StatusEnum>.GetEnumMemberValue(x.Data?.Status));
        }

        [Fact]
        public async void TestStringTemplate001()
        {
            var x = await client.GetTenderAsync("b6c1b8c0c2074bc8b9380cff823ee8e3");

            Assert.Equal(
                "6300000 UAH - 34520000 - 8 ×îâíè(Ìîòîğíèé ÷îâåí ïğîì³ğíèé) - (Ô²Ë²ß \"ÄÍÎÏÎÃËÈÁËŞÂÀËÜÍÈÉ ÔËÎÒ\" ÄÅĞÆÀÂÍÎÃÎ Ï²ÄÏĞÈªÌÑÒÂÀ \"ÀÄÌ²Í²ÑÒĞÀÖ²ß ÌÎĞÑÜÊÈÕ ÏÎĞÒ²Â ÓÊĞÀ¯ÍÈ\")",
                StringTemplate.ToString(@"{Value.String} - {Title} - ({ProcuringEntity.Name})", x.Data));
        }      

        [Fact]
        public async void TestTenderDocuments()
        {
            var x = await client.GetTenderDocumentsAsync("62a722e0afcb42eea8dd2c57f8c868f4");
        }
    }
}
