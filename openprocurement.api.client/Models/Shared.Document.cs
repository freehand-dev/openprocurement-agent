using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class TenderDocument
    {
        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("documentOf")]
        public string DocumentOf { get; set; }

        [JsonPropertyName("datePublished")]
        public DateTime DatePublished { get; set; }

        [JsonPropertyName("dateModified")]
        public DateTime DateModified { get; set; }

        [JsonPropertyName("relatedItem")]
        public string RelatedItem { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
