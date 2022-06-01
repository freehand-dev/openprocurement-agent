using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class NavigationPage
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("uri")]
        public Uri Uri { get; set; }


        [JsonConverter(typeof(JsonConverter.DateTimeOffsetJsonConverter))]
        [JsonPropertyName("offset")]
        public DateTimeOffset Offset { get; set; }
    }
}
