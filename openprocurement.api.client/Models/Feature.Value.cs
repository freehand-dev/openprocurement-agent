using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class FeatureValue
    {
        /// <summary>
        /// Value of the feature.
        /// </summary>
        [JsonPropertyName("value")]
        public float Value { get; set; }

        /// <summary>
        /// Title of the feature value.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Description of the feature value.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
