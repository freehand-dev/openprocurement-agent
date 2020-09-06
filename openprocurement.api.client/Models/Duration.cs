using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    /// <summary>
    /// https://prozorro-api-docs.readthedocs.io/en/master/standard/milestone.html#duration
    /// </summary>
    public class Duration
    {
        public enum TypeEnum
        {
            Working,
            Banking,
            Calendar
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("days")]
        public int Days { get; set; }
    }
}
