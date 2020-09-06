using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class Identifier
    {
        /// <summary>
        /// OpenContracting Description: Organization identifiers be drawn from an existing identification scheme. This field is used to indicate the scheme or codelist in which the identifier will be found. This value should be drawn from the Organization Identifier Scheme.
        /// </summary>
        [JsonPropertyName("scheme")]
        public string Scheme { get; set; }

        /// <summary>
        /// OpenContracting Description: The identifier of the organization in the selected scheme.
        /// The allowed codes are the ones found in “Organisation Registration Agency” codelist of IATI Standard with addition of UA-EDR code for organizations registered in Ukraine (EDRPOU and IPN).
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// OpenContracting Description: The legally registered name of the organization.
        /// </summary>
        [JsonPropertyName("legalName")]
        public string LegalName { get; set; }

        /// <summary>
        /// OpenContracting Description: A URI to identify the organization, such as those provided by Open Corporates or some other relevant URI provider. This is not for listing the website of the organization: that can be done through the url field of the Organization contact point.
        /// </summary>
        [JsonPropertyName("uri")]
        public Uri Uri { get; set; }

    }
}
