using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    /// <summary>
    /// https://prozorro-api-docs.readthedocs.io/en/master/standard/planrelation.html#planrelation
    /// </summary>
    public class PlanRelation
    {
        /// <summary>
        /// id of the linked plan object. See Tender creation from a procurement plan
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
