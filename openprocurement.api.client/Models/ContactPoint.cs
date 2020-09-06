using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class ContactPoint
    {
        /// <summary>
        /// OpenContracting Description: The name of the contact person, department, or contact point, for correspondence relating to this contracting process.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// OpenContracting Description: The e-mail address of the contact point/person.
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// OpenContracting Description: The telephone number of the contact point/person. This should include the international dialling code.
        /// </summary>
        [JsonPropertyName("telephone")]
        public string Telephone { get; set; }

        /// <summary>
        /// OpenContracting Description: The fax number of the contact point/person. This should include the international dialling code.
        /// </summary>
        [JsonPropertyName("faxNumber")]
        public string FaxNumber { get; set; }

        /// <summary>
        /// OpenContracting Description: A web address for the contact point/person.
        /// </summary>
        [JsonPropertyName("url")]
        public Uri Url { get; set; }

    }
}
