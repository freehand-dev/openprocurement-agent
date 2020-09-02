using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class TenderBase
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("dateModified")]
        public DateTime DateModified { get; set; }
    }
}
