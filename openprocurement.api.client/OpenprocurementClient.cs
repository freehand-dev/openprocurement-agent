using openprocurement.api.client.Exceptions;
using openprocurement.api.client.Extensions;
using openprocurement.api.client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace openprocurement.api.client
{
    public class OpenprocurementClient : HttpClient, IOpenprocurementClient
    {
        private readonly string _api_version = "2.5";
        private readonly string _api_uri = "https://public.api.openprocurement.org/api/{0}/";

        private Uri GetEndpoint()
        {
            return new Uri(String.Format(_api_uri, _api_version));
        }

        public OpenprocurementClient() :
            base()
        {
            this.DefaultRequestHeaders.Accept.Clear();
            this.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            this.DefaultRequestHeaders.Add("User-Agent", "Oleksandr Nazaruk");
        }

        public async Task<Models.TendersResponse> GetTendersAsync(DateTime offset, int limit = 0)
        {

            Dictionary<string, object> queryParams = new Dictionary<string, object>()
            {
                {"limit", (limit <= 0) ? 100 : limit}
            };

            if (offset != null)
            {
                queryParams.Add("offset", offset.ToString("o", System.Globalization.CultureInfo.InvariantCulture));
            }

            var httpResponse = await this.GetAsync(
                this.GetEndpoint().Append("tenders").Query(queryParams));

            // debug
            System.Diagnostics.Debug.WriteLine(
                this.GetEndpoint().Append("tenders").Query(queryParams).ToString());


            if (!httpResponse.IsSuccessStatusCode)
                throw new ErrorResponseException($"GetTenders response error, ReasonPhrase is {httpResponse.ReasonPhrase}", httpResponse);

            var contentStream = await httpResponse.Content.ReadAsStreamAsync();
            return await System.Text.Json.JsonSerializer.DeserializeAsync<Models.TendersResponse>(contentStream, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });

        }


        /// <summary>
        /// Reading the individual tender information
        /// https://public.api.openprocurement.org/api/2.5/tenders/98a4044accb640738e805a0bfe245034
        /// http://api-docs.openprocurement.org/en/latest/tenders.html?highlight=tenders
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<DataResponse<Tender>> GetTenderAsync(string Id)
        {
            var httpResponse = await this.GetAsync(
                this.GetEndpoint().Append("tenders").Append(Id));

            // debug
            System.Diagnostics.Debug.WriteLine(
                this.GetEndpoint().Append("tenders").Append(Id).ToString());

            if (!httpResponse.IsSuccessStatusCode)
                throw new ErrorResponseException($"GetTender with id { Id } response error, ReasonPhrase is {httpResponse.ReasonPhrase}", httpResponse);


            var contentStream = await httpResponse.Content.ReadAsStreamAsync();
            return await System.Text.Json.JsonSerializer.DeserializeAsync<DataResponse<Tender>>(contentStream, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });

        }

        /// <summary>
        /// Reading the tender documents list
        /// https://public.api.openprocurement.org/api/2.5/tenders/98a4044accb640738e805a0bfe245034/documents
        /// http://api-docs.openprocurement.org/en/latest/tenders.html?highlight=tenders
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<DataResponse<List<TenderDocument>>> GetTenderDocumentsAsync(string Id)
        {
            var httpResponse = await this.GetAsync(
                this.GetEndpoint().Append("tenders").Append(Id).Append("documents"));

            // debug
            System.Diagnostics.Debug.WriteLine(
                this.GetEndpoint().Append("tenders").Append(Id).Append("documents"));

            if (!httpResponse.IsSuccessStatusCode)
                throw new ErrorResponseException($"GetTenderDocuments with id { Id } response, ReasonPhrase is {httpResponse.ReasonPhrase}", httpResponse);

            var contentStream = await httpResponse.Content.ReadAsStreamAsync();
            return await System.Text.Json.JsonSerializer.DeserializeAsync<DataResponse<List<TenderDocument>>>(contentStream, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });

        }


        /// <summary>
        /// Reading the tender documents list
        /// https://public.api.openprocurement.org/api/2.5/tenders/62a722e0afcb42eea8dd2c57f8c868f4/contracts
        /// http://api-docs.openprocurement.org/en/latest/tenders.html?highlight=tenders
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<DataResponse<List<TenderContract>>> GetTenderContractsAsync(string Id)
        {
            var httpResponse = await this.GetAsync(
                this.GetEndpoint().Append("tenders").Append(Id).Append("contracts"));

            // debug
            System.Diagnostics.Debug.WriteLine(
                this.GetEndpoint().Append("tenders").Append(Id).Append("contracts"));

            if (!httpResponse.IsSuccessStatusCode)
                throw new ErrorResponseException($"GetTenderContracts with id { Id } response, ReasonPhrase is {httpResponse.ReasonPhrase}", httpResponse);

            var contentStream = await httpResponse.Content.ReadAsStreamAsync();
            return await System.Text.Json.JsonSerializer.DeserializeAsync<DataResponse<List<TenderContract>>>(contentStream, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });

        }

        public async Task<DataResponse<List<TenderAward>>> GetTenderAwardsAsync(string Id)
        {
            var httpResponse = await this.GetAsync(
                this.GetEndpoint().Append("tenders").Append(Id).Append("awards"));

            // debug
            System.Diagnostics.Debug.WriteLine(
                this.GetEndpoint().Append("tenders").Append(Id).Append("awards"));

            if (!httpResponse.IsSuccessStatusCode)
                throw new ErrorResponseException($"GetTenderAwardsAsync with id { Id } response, ReasonPhrase is {httpResponse.ReasonPhrase}", httpResponse);

            var contentStream = await httpResponse.Content.ReadAsStreamAsync();
            return await System.Text.Json.JsonSerializer.DeserializeAsync<DataResponse<List<TenderAward>>>(contentStream, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });

        }


        


    }
}
