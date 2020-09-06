using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class PlanOrganization
    {
        /// <summary>
        /// OpenContracting Description: The primary identifier for this organization.
        /// </summary>
        [JsonPropertyName("identifier")]
        public Identifier Identifier { get; set; }

        /// <summary>
        /// OpenContracting Description: The common name of the organization.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
