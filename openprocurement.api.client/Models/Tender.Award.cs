using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class TenderAward
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("suppliers")]
        public List<ContractSupplier> Suppliers { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("value")]
        public TenderValue Value { get; set; }
    }
}
