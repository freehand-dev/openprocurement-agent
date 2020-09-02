using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class TenderContract
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("documents")]
        public List<TenderDocument> Documents { get; set; }

        [JsonPropertyName("items")]
        public List<TenderItem> Items { get; set; }

        [JsonPropertyName("suppliers")]
        public List<ContractSupplier> Suppliers { get; set; }

        [JsonPropertyName("contractNumber")]
        public string ContractNumber { get; set; }

        [JsonPropertyName("period")]
        public PeriodDate Period { get; set; }

        [JsonPropertyName("dateSigned")]
        public DateTime DateSigned { get; set; }

        [JsonPropertyName("value")]
        public TenderValue Value { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("awardID")]
        public string AwardID{ get; set; }

        [JsonPropertyName("id")]
        public string Id{ get; set; }

        [JsonPropertyName("contractID")]
        public string ContractID { get; set; }

    }
}
