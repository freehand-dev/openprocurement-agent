using openprocurement.api.client.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace openprocurement.api.client.Exceptions
{

    [Serializable]
    public class ErrorResponseException : System.ApplicationException
    {

        public ErrorResponse ErrorResponse { get; }

        public ErrorResponseException()
        { 
            
        }

        public ErrorResponseException(string message, HttpResponseMessage httpResponse) : base(message) 
        {
            if (!httpResponse.IsSuccessStatusCode)
                if (httpResponse.Content != null)
                    this.ErrorResponse = ErrorResponseException.Deserialize(httpResponse).GetAwaiter().GetResult();
        }

        public ErrorResponseException(string message, HttpResponseMessage httpResponse, Exception inner): base(message, inner)  
        {
            if (!httpResponse.IsSuccessStatusCode)
                if (httpResponse.Content != null)
                    this.ErrorResponse = ErrorResponseException.Deserialize(httpResponse).GetAwaiter().GetResult();
        }

        protected ErrorResponseException(SerializationInfo info, StreamingContext context): base(info, context) { }

        public static async Task<ErrorResponse> Deserialize(HttpResponseMessage httpResponse)
        {
            var contentStream = await httpResponse.Content.ReadAsStreamAsync();
            return await System.Text.Json.JsonSerializer.DeserializeAsync<ErrorResponse>(contentStream, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
        }


    }
}
