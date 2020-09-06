using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class Organization
    {
        /// <summary>
        /// OpenContracting Description: The common name of the organization.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// OpenContracting Description: The common name of the organization.
        /// </summary>
        [JsonPropertyName("name_en")]
        public string NameEn { get; set; }

        /// <summary>
        /// OpenContracting Description: The primary identifier for this organization.
        /// </summary>
        [JsonPropertyName("identifier")]
        public Identifier Identifier { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("additionalIdentifiers")]
        public List<Identifier> AdditionalIdentifiers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("address")]
        public Address Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("contactPoint")]
        public ContactPoint ContactPoint { get; set; }
    }
}
