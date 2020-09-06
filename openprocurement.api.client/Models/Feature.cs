using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class Feature
    {
        public enum FeatureOfEnum
        {
            Tenderer,
            Lot,
            Item,
        }

        /// <summary>
        /// Code of the feature.
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("featureOf")]
        public string FeatureOf { get; set; }

        /// <summary>
        /// Id of related Item or Lot (only if the featureOf value is item or lot).
        /// </summary>
        [JsonPropertyName("relatedItem")]
        public string RelatedItem { get; set; }

        /// <summary>
        /// Title of the feature.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Description of the feature.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// List of values
        /// </summary>
        [JsonPropertyName("enum")]
        public List<FeatureValue> Enum { get; set; }

    }
}
