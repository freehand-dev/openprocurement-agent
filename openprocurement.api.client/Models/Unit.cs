using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class Unit
    {
        /// <summary>
        /// UN/CEFACT Recommendation 20 unit code.
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// OpenContracting Description: Name of the unit
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

    }
}
