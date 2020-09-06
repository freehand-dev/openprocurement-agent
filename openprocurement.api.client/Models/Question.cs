using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    /// <summary>
    /// https://prozorro-api-docs.readthedocs.io/en/master/standard/question.html#question
    /// </summary>
    public class Question
    {
        public enum QuestionOfEnum
        {
            Tender,
            Item,
            Lot,
        }

        /// <summary>
        /// auto-generated
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Who is asking a question (contactPoint - person, identification - organization that person represents).
        /// </summary>
        [JsonPropertyName("author")]
        public Organization Author { get; set; }

        /// <summary>
        /// Title of the question.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Description of the question.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Date of posting.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Date when answer has been provided.
        /// </summary>
        [JsonPropertyName("dateAnswered")]
        public DateTime DateAnswered { get; set; }

        /// <summary>
        /// Answer for the question asked.
        /// </summary>
        [JsonPropertyName("answer")]
        public string Answer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("questionOf")]
        public string QuestionOf { get; set; }

        /// <summary>
        /// Id of related Lot or Item.
        /// </summary>
        [JsonPropertyName("relatedItem")]
        public string RelatedItem { get; set; }
    }
}
