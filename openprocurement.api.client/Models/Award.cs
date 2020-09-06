using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    /// <summary>
    /// https://prozorro-api-docs.readthedocs.io/en/master/standard/award.html#award
    /// </summary>
    public class Award
    {
        /// <summary>
        /// OpenContracting Description: The identifier for this award.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// The Id of a bid that the award relates to.
        /// absent in Reporting, negotiation procurement procedure and negotiation procedure:
        /// </summary>
        [JsonPropertyName("bid_id")]
        public string BidId { get; set; }

        /// <summary>
        /// OpenContracting Description: Award title.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// OpenContracting Description: Award description.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// OpenContracting Description: The current status of the award drawn from the awardStatus codelist.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// OpenContracting Description: The date of the contract award.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// List of BusinessOrganization objects, auto-generated, read-only
        /// </summary>
        [JsonPropertyName("suppliers")]
        public List<BusinessOrganization> Suppliers { get; set; }

        /// <summary>
        /// OpenContracting Description: The total value of this award.
        /// </summary>
        [JsonPropertyName("value")]
        public Value Value { get; set; }

        /// <summary>
        /// OpenContracting Description: All documents and attachments related to the award, including any notices.
        /// </summary>
        [JsonPropertyName("documents")]
        public List<Document> Documents { get; set; }

        /// <summary>
        /// List of Complaint objects
        /// </summary>
        [JsonPropertyName("complaints")]
        public List<Complaint> Complaints { get; set; }

        /// <summary>
        /// The timeframe when complaints can be submitted.
        /// </summary>
        [JsonPropertyName("complaintPeriod")]
        public Period ComplaintPeriod { get; set; }

        /// <summary>
        /// Id of related Lot.
        /// </summary>
        [JsonPropertyName("lotID")]
        public string LotID { get; set; }

        /// <summary>
        /// Confirms compliance of eligibility criteria set by the procuring entity in the tendering documents.
        /// </summary>
        [JsonPropertyName("eligible")]
        public bool Eligible { get; set; }

        /// <summary>
        /// Confirms the absence of grounds for refusal to participate in accordance with Article 17 of the Law of Ukraine “On Public Procurement”.
        /// </summary>
        [JsonPropertyName("qualified")]
        public bool Qualified { get; set; }

        /// <summary>
        /// The text field of any length that contains information about subcontractor.
        /// </summary>
        [JsonPropertyName("subcontractingDetails")]
        public string SubcontractingDetails { get; set; }
    }
}
