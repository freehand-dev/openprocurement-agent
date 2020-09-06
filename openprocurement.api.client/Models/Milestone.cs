using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class Milestone
    {
        public enum CodeEnum {
            Prepayment,
            Postpayment
        }

        public enum TypeEnum
        {
            Financing
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// integer, required, non negative
        /// </summary>
        [JsonPropertyName("sequenceNumber")]
        public int SequenceNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("duration")]
        public Duration Duration { get; set; }

        /// <summary>
        /// Sum of all tender (or lot) milestones should be 100
        /// </summary>
        [JsonPropertyName("percentage")]
        public float Percentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("type")]
        public TypeEnum Type { get; set; }

        /// <summary>
        /// auto-generated
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Id of related Lot.
        /// </summary>
        [JsonPropertyName("idrelatedLot")]
        public string RelatedLot { get; set; }

    }
}
