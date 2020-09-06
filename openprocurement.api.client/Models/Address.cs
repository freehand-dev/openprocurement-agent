using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class Address
    {
        /// <summary>
        /// OpenContracting Description: The street address. For example, 1600 Amphitheatre Pkwy.
        /// </summary>
        [JsonPropertyName("streetAddress")]
        public string StreetAddress { get; set; }

        /// <summary>
        /// OpenContracting Description: The locality. For example, Mountain View.
        /// </summary>
        [JsonPropertyName("locality")]
        public string Locality { get; set; }

        /// <summary>
        /// OpenContracting Description: The region. For example, CA.
        /// </summary>
        [JsonPropertyName("region")]
        public string Region { get; set; }

        /// <summary>
        /// OpenContracting Description: The postal code. For example, 94043.
        /// </summary>
        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }

        /// <summary>
        /// OpenContracting Description: The country name. For example, United States.
        /// </summary>
        [JsonPropertyName("countryName")]
        public string CountryName { get; set; }





    }
}
