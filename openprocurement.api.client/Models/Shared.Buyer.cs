using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class Buyer
    {
        [JsonPropertyName("identifier")]
        public Identifier Identifier { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
