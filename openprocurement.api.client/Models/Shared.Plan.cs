using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class Plan
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
