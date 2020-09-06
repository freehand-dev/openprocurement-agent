using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    /// <summary>
    /// https://prozorro-api-docs.readthedocs.io/en/master/standard/complaintpost.html#complaintpost
    /// </summary>
    public class ComplaintPost
    {
        /// <summary>
        /// Title of the post.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Description of the post.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// List of Document objects
        /// </summary>
        [JsonPropertyName("documents")]
        public List<Document> Documents { get; set; }

        /// <summary>
        /// Id of related ComplaintPost.
        /// </summary>
        [JsonPropertyName("relatedPost")]
        public string RelatedPost { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("recipient")]
        public string Recipient { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("author")]
        public string Author { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("datePublished")]
        public DateTime DatePublished { get; set; }
    }
}
