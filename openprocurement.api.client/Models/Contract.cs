using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    /// <summary>
    /// https://prozorro-api-docs.readthedocs.io/en/master/standard/contract.html#contract
    /// </summary>
    public class Contract
    {
        /// <summary>
        /// OpenContracting Description: The identifier for this contract.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// OpenContracting Description: The Award.id against which this contract is being issued.
        /// </summary>
        [JsonPropertyName("awardID")]
        public string AwardID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("contractID")]
        public string ContractID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("contractNumber")]
        public string ContractNumber { get; set; }

        /// <summary>
        /// OpenContracting Description: Contract title
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// OpenContracting Description: Contract description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// OpenContracting Description: The current status of the contract.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// OpenContracting Description: The start and end date for the contract.
        /// </summary>
        [JsonPropertyName("period")]
        public Period Period { get; set; }

        /// <summary>
        /// OpenContracting Description: The goods, services, and any intangible outcomes in this contract. Note: If the items are the same as the award do not repeat.
        /// </summary>
        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }

        /// <summary>
        /// List of  objects, auto-generated, read-only
        /// </summary>
        [JsonPropertyName("suppliers")]
        public List<BusinessOrganization> Suppliers { get; set; }

        /// <summary>
        /// OpenContracting Description: The total value of this contract.
        /// </summary>
        [JsonPropertyName("value")]
        public ContractValue Value { get; set; }

        /// <summary>
        /// OpenContracting Description: The date when the contract was signed. In the case of multiple signatures, the date of the last signature.
        /// </summary>
        [JsonPropertyName("dateSigned")]
        public DateTime DateSigned { get; set; }

        /// <summary>
        /// OpenContracting Description: All documents and attachments related to the contract, including any notices.
        /// </summary>
        [JsonPropertyName("documents")]
        public List<Document> Documents { get; set; }

        /// <summary>
        /// The date when the contract was changed or activated.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

    }
}
