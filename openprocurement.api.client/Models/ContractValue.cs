using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class ContractValue: Value
    {
        /// <summary>
        /// OpenContracting Description: Amount as a number.
        /// </summary>
        [JsonPropertyName("amountNet")]
        public float AmountNet { get; set; }
    }
}
