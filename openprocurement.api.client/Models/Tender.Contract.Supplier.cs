using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class ContractSupplier
    {
        [JsonPropertyName("scale")]
        public string Scale { get; set; }

        [JsonPropertyName("contactPoint")]
        public ContactPoint ContactPoint { get; set; }

        [JsonPropertyName("identifier")]
        public Identifier Identifier { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        public Address Address { get; set; }
    }
}
