using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class DataResponse<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
