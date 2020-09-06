using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class Guarantee
    {
        /// <summary>
        /// OpenContracting Description: Amount as a number.
        /// </summary>
        [JsonPropertyName("amount")]
        public float Amount { get; set; }

        /// <summary>
        /// OpenContracting Description: The currency in 3-letter ISO 4217 format.
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = "UAH";
    }
}
