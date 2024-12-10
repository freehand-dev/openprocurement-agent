using openprocurement.api.client.Exceptions;
using openprocurement.api.client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace openprocurement.api.client
{

    /// <summary>
    /// http://api-docs.openprocurement.org/en
    /// </summary>
    public class OpenprocurementClient : IOpenprocurementClient
    {
        private readonly string _api_version = "2.5";
        private readonly string _api_uri = "https://public.api.openprocurement.org/api/{0}/";

        private readonly HttpClient _httpClient;

        JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            Converters = {
                new JsonStringEnumConverter( JsonNamingPolicy.CamelCase)
            },
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };

        public OpenprocurementClient(HttpClient httpClient)
        {
            this._httpClient = httpClient;
            this._httpClient.BaseAddress = new Uri(String.Format(_api_uri, _api_version));
            this._httpClient.DefaultRequestHeaders.Accept.Clear();
            this._httpClient.DefaultRequestHeaders.Add("User-Agent", "OleksandrNazaruk");
            this._httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string BuildQuerystring(Dictionary<string, object> query = null) =>
            query == null 
                ? string.Empty 
                : $"?{ string.Join("&", query.Select(kvp => string.Format("{0}={1}", kvp.Key, HttpUtility.UrlEncode(Convert.ToString(kvp.Value))))) }";
            
        /// <summary>
        /// http://api-docs.openprocurement.org/en/latest/tenders.html
        /// </summary>
        /// <param name="offset">This is the parameter you have to add to the original request you made to get next page.</param>
        /// <param name="limit">You can control the number of data entries in the tenders feed (batch size) with limit parameter. If not specified, data is being returned in batches of 100 elements.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException"></exception>
        public async Task<Models.TendersResponse> GetTendersAsync(DateTimeOffset? offset, int? limit, CancellationToken cancellationToken)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            if (limit.HasValue && limit > 0)
            {
                parameters.Add("limit", limit);
            }
            if (offset.HasValue)
            {
                parameters.Add("offset", offset.Value.ToUnixTimeSeconds());
            }

            var httpResponse = await this._httpClient.GetAsync($"tenders{ BuildQuerystring(parameters) }", cancellationToken);
            if (!httpResponse.IsSuccessStatusCode)
                throw new ErrorResponseException($"GetTenders response error, ReasonPhrase is {httpResponse.ReasonPhrase}", httpResponse);
            var contentStream = await httpResponse.Content.ReadAsStreamAsync();
            return await System.Text.Json.JsonSerializer.DeserializeAsync<Models.TendersResponse>(contentStream, _jsonSerializerOptions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nextPage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException"></exception>
        public async Task<Models.TendersResponse> GetTendersAsync(NavigationPage nextPage, CancellationToken cancellationToken)
        {
            var httpResponse = await this._httpClient.GetAsync(nextPage.Path, cancellationToken);
            if (!httpResponse.IsSuccessStatusCode)
                throw new ErrorResponseException($"GetTenders response error, ReasonPhrase is {httpResponse.ReasonPhrase}", httpResponse);
            var contentStream = await httpResponse.Content.ReadAsStreamAsync();
            return await System.Text.Json.JsonSerializer.DeserializeAsync<Models.TendersResponse>(contentStream, _jsonSerializerOptions);
        }

        /// <summary>
        /// Reading the individual tender information
        /// https://public.api.openprocurement.org/api/2.5/tenders/98a4044accb640738e805a0bfe245034
        /// http://api-docs.openprocurement.org/en/latest/tenders.html?highlight=tenders
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<DataResponse<Tender>> GetTenderAsync(string Id, CancellationToken cancellationToken)
        {
            var httpResponse = await this._httpClient.GetAsync($"tenders/{Id}", cancellationToken);
            if (!httpResponse.IsSuccessStatusCode)
                throw new ErrorResponseException($"GetTender with id { Id } response error, ReasonPhrase is {httpResponse.ReasonPhrase}", httpResponse);
            var contentStream = await httpResponse.Content.ReadAsStreamAsync();
            return await System.Text.Json.JsonSerializer.DeserializeAsync<DataResponse<Tender>>(contentStream, _jsonSerializerOptions);
        }

        /// <summary>
        /// Reading the tender documents list
        /// https://public.api.openprocurement.org/api/2.5/tenders/98a4044accb640738e805a0bfe245034/documents
        /// http://api-docs.openprocurement.org/en/latest/tenders.html?highlight=tenders
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<DataResponse<List<Document>>> GetTenderDocumentsAsync(string Id, CancellationToken cancellationToken)
        {
            var httpResponse = await this._httpClient.GetAsync($"tenders/{Id}/documents", cancellationToken);
            if (!httpResponse.IsSuccessStatusCode)
                throw new ErrorResponseException($"GetTenderDocuments with id { Id } response, ReasonPhrase is {httpResponse.ReasonPhrase}", httpResponse);
            var contentStream = await httpResponse.Content.ReadAsStreamAsync();
            return await System.Text.Json.JsonSerializer.DeserializeAsync<DataResponse<List<Document>>>(contentStream, _jsonSerializerOptions);
        }


        /// <summary>
        /// Reading the tender documents list
        /// https://public.api.openprocurement.org/api/2.5/tenders/62a722e0afcb42eea8dd2c57f8c868f4/contracts
        /// http://api-docs.openprocurement.org/en/latest/tenders.html?highlight=tenders
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<DataResponse<List<Contract>>> GetTenderContractsAsync(string Id, CancellationToken cancellationToken)
        {
            var httpResponse = await this._httpClient.GetAsync($"tenders/{Id}/contracts", cancellationToken);
            if (!httpResponse.IsSuccessStatusCode)
                throw new ErrorResponseException($"GetTenderContracts with id { Id } response, ReasonPhrase is {httpResponse.ReasonPhrase}", httpResponse);
            var contentStream = await httpResponse.Content.ReadAsStreamAsync();
            return await System.Text.Json.JsonSerializer.DeserializeAsync<DataResponse<List<Contract>>>(contentStream, _jsonSerializerOptions);
        }

        public async Task<DataResponse<List<Award>>> GetTenderAwardsAsync(string Id, CancellationToken cancellationToken)
        {
            var httpResponse = await this._httpClient.GetAsync($"tenders/{Id}/awards", cancellationToken);
            if (!httpResponse.IsSuccessStatusCode)
                throw new ErrorResponseException($"GetTenderAwardsAsync with id { Id } response, ReasonPhrase is {httpResponse.ReasonPhrase}", httpResponse);
            var contentStream = await httpResponse.Content.ReadAsStreamAsync();
            return await System.Text.Json.JsonSerializer.DeserializeAsync<DataResponse<List<Award>>>(contentStream, _jsonSerializerOptions);
        }


        


    }
}
