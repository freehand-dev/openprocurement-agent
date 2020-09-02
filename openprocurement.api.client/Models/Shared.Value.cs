using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class TenderValue
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("amountNet")]
        public decimal AmountNet { get; set; }

        [JsonPropertyName("valueAddedTaxIncluded")]
        public bool ValueAddedTaxIncluded { get; set; }
    }
}
