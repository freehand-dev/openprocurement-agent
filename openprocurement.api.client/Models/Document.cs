using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    /// <summary>
    /// https://prozorro-api-docs.readthedocs.io/en/master/standard/document.html#document
    /// </summary>
    public class Document
    {
        public enum DocumentOfEnum
        {
            Tender,
            Contract,
            Change,
            Item,
            Lot
        }

        /// <summary>
        /// auto-generated
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Type of Document
        /// </summary>
        [JsonPropertyName("documentType")]
        public string DocumentType { get; set; }

        /// <summary>
        /// OpenContracting Description: The document title.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// OpenContracting Description: A short description of the document. In the event the document is not accessible online, the description field can be used to describe arrangements for obtaining a copy of the document.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// OpenContracting Description: The format of the document taken from the IANA Media Types code list, with the addition of one extra value for ‘offline/print’, used when this document entry is being used to describe the offline publication of a document.
        /// </summary>
        [JsonPropertyName("format")]
        public string Format { get; set; }

        /// <summary>
        /// OpenContracting Description: Direct link to the document or attachment.
        /// </summary>
        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        /// <summary>
        /// OpenContracting Description: The date on which the document was first published.
        /// </summary>
        [JsonPropertyName("datePublished")]
        public DateTime DatePublished { get; set; }

        /// <summary>
        /// OpenContracting Description: Date that the document was last modified
        /// </summary>
        [JsonPropertyName("dateModified")]
        public DateTime DateModified { get; set; }

        /// <summary>
        /// OpenContracting Description: Specifies the language of the linked document using either two-digit ISO 639-1, or extended BCP47 language tags.
        /// </summary>
        [JsonPropertyName("language")]
        public string Language { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("documentOf")]
        public string DocumentOf { get; set; }

        /// <summary>
        /// Id of related Contract, Change, Lot or Item.
        /// </summary>
        [JsonPropertyName("relatedItem")]
        public string RelatedItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("hash")]
        public string Hash { get; set; }
    }
}
