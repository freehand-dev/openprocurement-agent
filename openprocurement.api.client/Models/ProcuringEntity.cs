using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class ProcuringEntity
    {
        public enum KindEnum
        {
            /// <summary>
            /// Procuring entity (general)
            /// </summary>
            General,

            /// <summary>
            /// Procuring entity that operates in certain spheres of economic activity
            /// </summary>
            Special,

            /// <summary>
            ///  Procuring entity that conducts procurement for the defense needs
            /// </summary>
            Defense,

            /// <summary>
            /// Legal persons that are not procuring entities in the sense of the Law, but are state, utility, public enterprises, economic partnerships or associations of enterprises in which state or public utility share is 50 percent or more
            /// </summary>
            Other
        }

        /// <summary>
        /// OpenContracting Description: The common name of the organization.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// OpenContracting Description: The primary identifier for this organization.
        /// </summary>
        [JsonPropertyName("identifier")]
        public Identifier Identifier { get; set; }

        /// <summary>
        /// List of Identifier objects
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

        /// <summary>
        /// Type of procuring entity
        /// </summary>
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

    }
}
