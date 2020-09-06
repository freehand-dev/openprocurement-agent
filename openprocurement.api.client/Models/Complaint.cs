using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    /// <summary>
    /// https://prozorro-api-docs.readthedocs.io/en/master/standard/complaint.html#complaint
    /// </summary>
    public class Complaint
    {
        /// <summary>
        /// uid, auto-generated
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Organization filing a complaint (contactPoint - person, identification - organization that person represents).
        /// </summary>
        [JsonPropertyName("author")]
        public Organization Author { get; set; }

        /// <summary>
        /// Title of the complaint.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Description of the issue.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Date of posting.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Date when claim was submitted.
        /// </summary>
        [JsonPropertyName("dateSubmitted")]
        public DateTime DateSubmitted { get; set; }

        /// <summary>
        /// Date when Procuring entity answered the claim.
        /// </summary>
        [JsonPropertyName("dateAnswered")]
        public DateTime DateAnswered { get; set; }

        /// <summary>
        /// Date of claim to complaint escalation.
        /// </summary>
        [JsonPropertyName("dateEscalated")]
        public DateTime DateEscalated { get; set; }

        /// <summary>
        /// Date of complaint decision.
        /// </summary>
        [JsonPropertyName("dateDecision")]
        public DateTime DateDecision { get; set; }

        /// <summary>
        /// Date of cancelling.
        /// </summary>
        [JsonPropertyName("dateCanceled")]
        public DateTime DateCanceled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Resolution of Procuring entity.
        /// </summary>
        [JsonPropertyName("resolution")]
        public string Resolution { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("resolutionType")]
        public string ResolutionType { get; set; }

        /// <summary>
        /// Claim is satisfied?
        /// </summary>
        [JsonPropertyName("satisfied")]
        public bool Satisfied { get; set; }

        /// <summary>
        /// Reviewer decision.
        /// </summary>
        [JsonPropertyName("decision")]
        public string Decision { get; set; }

        /// <summary>
        /// Cancellation reason.
        /// </summary>
        [JsonPropertyName("cancellationReason")]
        public string CancellationReason { get; set; }

        /// <summary>
        /// List of Document objects
        /// </summary>
        [JsonPropertyName("documents")]
        public List<Document> Documents { get; set; }

        /// <summary>
        /// Id of related Lot.
        /// </summary>
        [JsonPropertyName("relatedLot")]
        public string RelatedLot { get; set; }

        /// <summary>
        /// Tenderer action.
        /// </summary>
        [JsonPropertyName("tendererAction")]
        public string TendererAction { get; set; }

        /// <summary>
        /// Date of tenderer action.
        /// </summary>
        [JsonPropertyName("tendererActionDate")]
        public DateTime TendererActionDate { get; set; }

        /// <summary>
        /// List of ComplaintPost objects
        /// </summary>
        [JsonPropertyName("posts")]
        public List<ComplaintPost> Posts { get; set; }

        /// <summary>
        /// Amount to be paid to activate this complaint. See Complaints Payments
        /// </summary>
        [JsonPropertyName("value")]
        public Guarantee Value { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("rejectReason")]
        public string RejectReason { get; set; }

        /// <summary>
        ///  Claim is satisfied?
        /// </summary>
        [JsonPropertyName("acceptance")]
        public bool Acceptance { get; set; }

        /// <summary>
        /// Reject reason description.
        /// </summary>
        [JsonPropertyName("rejectReasonDescription")]
        public string RejectReasonDescription { get; set; }

        /// <summary>
        /// Date of review.
        /// </summary>
        [JsonPropertyName("reviewDate")]
        public DateTime ReviewDate { get; set; }

        /// <summary>
        /// Place of review.
        /// </summary>
        [JsonPropertyName("reviewPlace")]
        public string ReviewPlace { get; set; }
    }
}
