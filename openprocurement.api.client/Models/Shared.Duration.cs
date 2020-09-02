using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class Duration
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("days")]
        public int Days { get; set; }
    }
}
