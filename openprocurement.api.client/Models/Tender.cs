using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace openprocurement.api.client.Models
{
    /// <summary>
    /// https://prozorro-api-docs.readthedocs.io/en/master/standard/tender.html
    /// </summary>
    public class Tender: TenderBase
    {
        #region 'no usage'
        /// <summary>
        /// 
        /// </summary>
        public enum StatusEnum
        {
            /// <summary>
            /// Active tender (default)
            /// </summary>
            [EnumMember(Value = "active")]
            Active,

            /// <summary>
            /// Enquiries period (enquiries)
            /// </summary>
            [EnumMember(Value = "active.enquiries")]
            ActiveEnquiries,

            /// <summary>
            /// Tendering period (tendering)
            /// </summary>
            [EnumMember(Value = "active.tendering")]
            ActiveTendering,

            /// <summary>
            /// Auction period (auction)
            /// </summary>
            [EnumMember(Value = "active.auction")]
            ActiveAuction,

            /// <summary>
            /// Winner qualification (qualification)
            /// </summary>
            [EnumMember(Value = "active.qualification")]
            ActiveQualification,

            /// <summary>
            /// Pre qulification period
            /// </summary>
            [EnumMember(Value = "active.pre-qualification")]
            ActivePreQualification,

            /// <summary>
            /// Standstill before auction
            /// </summary>
            [EnumMember(Value = "active.pre-qualification.stand-still")]
            ActivePreQualificationStandStill,

            /// <summary>
            /// Standstill period (standstill)
            /// </summary>
            [EnumMember(Value = "active.awarded")]
            ActiveAwarded,

            /// <summary>
            /// Unsuccessful tender (unsuccessful)
            /// </summary>
            [EnumMember(Value = "unsuccessful")]
            Unsuccessful,

            /// <summary>
            /// Complete tender (complete)
            /// </summary>
            [EnumMember(Value = "complete")]
            Complete,

            /// <summary>
            /// Cancelled tender (cancelled)
            /// </summary>
            [EnumMember(Value = "cancelled")]
            Cancelled,

            /// <summary>
            /// ProcuringEntity creats draft of procedure, where should be specified procurementMethodType - closeFrameworkAgreementSelectionUA, procurementMethod - selective. One lot structure procedure. Also ProcuringEntity should specify agreement:id, items, title, description and features, if needed.
            /// </summary>
            [EnumMember(Value = "draft")]
            Draft,

            /// <summary>
            /// ProcuringEntity changes status of procedure from ‘draft’ to ‘draft.pending’ to make the system check provided information and pull up necassery information from Agreement in cfaua.
            /// </summary>
            [EnumMember(Value = "draft.pending")]
            DraftPending,

            /// <summary>
            /// Terminal status. System moves procedure to ‘draft.unsuccessful’ status if at least one of the checks is failed.
            /// </summary>
            [EnumMember(Value = "draft.unsuccessful")]
            DraftUnsuccessful,
        }
        #endregion

        /// <summary>
        /// The name of the tender, displayed in listings. You can include the following items:
        /// - tender code(in procuring organization management system)        
        /// - periodicity of the tender(annual, quarterly, etc.)
        /// - item being procured
        /// - some other info
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// The name of the tender, displayed in listings. (multilingual)
        /// </summary>
        [JsonPropertyName("title_en")]
        public string TitleEng { get; set; }

        /// <summary>
        /// Detailed description of tender.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description{ get; set; }

        /// <summary>
        /// Detailed description of tender. (multilingual)
        /// </summary>
        [JsonPropertyName("description_en")]
        public string DescriptionEng { get; set; }

        /// <summary>
        /// The tender identifier to refer tender to in “paper” documentation.
        /// OpenContracting Description: TenderID should always be the same as the OCID.
        /// It is included to make the flattened data structure more convenient.
        /// </summary>
        [JsonPropertyName("tenderID")]
        public string TenderID { get; set; }

        /// <summary>
        /// Organization conducting the tender.
        /// OpenContracting Description: The entity managing the procurement, which may be different from the buyer who is paying / using the items being procured.
        /// </summary>
        [JsonPropertyName("procuringEntity")]
        public ProcuringEntity ProcuringEntity { get; set; }

        /// <summary>
        /// Procurement Method of the Tender.
        /// </summary>
        [JsonPropertyName("procurementMethod")]
        public string ProcurementMethod { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("procurementMethodType")]
        public string ProcurementMethodType { get; set; }

        /// <summary>
        /// Total available tender budget. Bids greater then value will be rejected.
        /// OpenContracting Description: The total estimated value of the procurement.
        /// </summary>
        [JsonPropertyName("value")]
        public Value Value { get; set; }

        /// <summary>
        /// Bid guarantee
        /// </summary>
        [JsonPropertyName("guarantee")]
        public Guarantee Guarantee { get; set; }

        /// <summary>
        /// Date, auto-generated	
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Date of tender announcement.	
        /// </summary>
        [JsonPropertyName("noticePublicationDate")]
        public DateTime NoticePublicationDate { get; set; }
        
        /// <summary>
        /// List that contains single item being procured.
        /// OpenContracting Description: The goods and services to be purchased, broken into line items wherever possible. Items should not be duplicated, but a quantity of 2 specified instead.
        /// </summary>
        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }

        /// <summary>
        /// Features of tender.
        /// </summary>
        [JsonPropertyName("features")]
        public List<Feature> Features { get; set; }

        /// <summary>
        /// OpenContracting Description: All documents and attachments related to the tender.
        /// </summary>
        [JsonPropertyName("documents")]
        public List<Document> Documents { get; set; }

        /// <summary>
        /// Questions to procuringEntity and answers to them.
        /// </summary>
        [JsonPropertyName("questions")]
        public List<Question> questions { get; set; }

        /// <summary>
        /// The minimal step of auction (reduction)
        /// </summary>
        [JsonPropertyName("minimalStep")]
        public Value MinimalStep { get; set; }

        /// <summary>
        /// Period when bids can be submitted. At least endDate has to be provided.
        /// OpenContracting Description: The period when the tender is open for submissions. The end date is the closing date for tender submissions.
        /// </summary>
        [JsonPropertyName("tenderPeriod")]
        public Period TenderPeriod { get; set; }

        /// <summary>
        /// OpenContracting Description: The primary category describing the main object of the tender.
        /// goods:	The primary object of this tender involves physical or electronic goods or supplies.
        /// services:	The primary object of this tender involves construction, repair, rehabilitation, demolition, restoration or maintenance of some asset or infrastructure.
        /// works:	The primary object of this tender involves professional services of some form, generally contracted for on the basis of measurable outputs or deliverables.
        /// </summary>
        [JsonPropertyName("mainProcurementCategory")]
        public string MainProcurementCategory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("complaintPeriod")]
        public Period ComplaintPeriod { get; set; }

        /// <summary>
        /// Period when Auction is conducted.
        /// </summary>
        [JsonPropertyName("auctionPeriod")]
        public Period AuctionPeriod { get; set; }

        /// <summary>
        /// Period when questions are allowed. At least endDate has to be provided.
        /// OpenContracting Description: The period during which enquiries may be made and will be answered.
        /// </summary>
        [JsonPropertyName("enquiryPeriod")]
        public EnquiryPeriod EnquiryPeriod { get; set; }

        /// <summary>
        /// Status of the Tender.
        /// </summary>
        [JsonPropertyName("status")]
        ///[JsonConverter(typeof(JsonEnumConverter<StatusEnum>))]
        ///JsonEnumConverter<Tender.StatusEnum>.GetEnumMemberValue(message.Status)
        public string Status { get; set; }

        /// <summary>
        /// List of PlanOrganization objects, required at least 1 object in case of the central procurement kind
        /// Identifications of the subjects in whose interests the purchase is made
        /// </summary>
        [JsonPropertyName("buyers")]
        public List<PlanOrganization> Buyers { get; set; }

        /// <summary>
        /// allow value electronicAuction
        /// </summary>
        [JsonPropertyName("submissionMethod")]
        public string SubmissionMethod { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("next_check")]
        public DateTime NextCheck { get; set; }

        /// <summary>
        /// allow value lowestCost
        /// </summary>
        [JsonPropertyName("awardCriteria")]
        public string AwardCriteria { get; set; }

        /// <summary>
        /// This period consists of qualification and 10 days of stand still period.
        /// OpenContracting Description: Period when qualification can be submitted with stand still period.
        /// </summary>
        [JsonPropertyName("qualificationPeriod")]
        public Period QualificationPeriod { get; set; }

        /// <summary>
        /// List of PlanRelation objects.
        /// </summary>
        [JsonPropertyName("plans")]
        public List<PlanRelation> Plans { get; set; }

        /// <summary>
        /// List of Milestone objects.
        /// </summary>
        [JsonPropertyName("milestones")]
        public List<Milestone> Milestones { get; set; }

        /// <summary>
        /// auto-generated
        /// </summary>
        [JsonPropertyName("owner")]
        public string Owner { get; set; }

        /// <summary>
        /// All qualifications (disqualifications and awards).
        /// </summary>
        [JsonPropertyName("awards")]
        public List<Award> Awards { get; set; }

        /// <summary>
        /// List of Contract objects
        /// </summary>
        [JsonPropertyName("contracts")]
        public List<Contract> Contracts { get; set; }


        public StringBuilder ToHTML()
        {
            var html = new StringBuilder();

            html.Append("<html>");
            html.Append("	<head>");
            html.Append("		<style>");
            html.Append("			body, td{");
            html.Append("				font-size: 14px;");
            html.Append("				vertical-align: top;");
            html.Append("			}");
            html.Append("			table.border{");
            html.Append("				border-collapse: collapse;");
            html.Append("			}");
            html.Append("			table.border td{");
            html.Append("				border: 1px solid #888;");
            html.Append("			}");
            html.Append("			.small{");
            html.Append("				font-size:12px;");
            html.Append("			}");
            html.Append("			h2{");
            html.Append("				margin-bottom:0px;");
            html.Append("			}");
            html.Append("			center{");
            html.Append("				text-align: center;");
            html.Append("			}");
            html.Append("		</style>");
            html.Append("		<meta name=\"robots\" content=\"noindex\" />");
            html.Append("	</head>");
            html.Append("	<body>");

            html.Append(this.ToHTMLBody());

            html.Append("	</body>");
            html.Append("</html>");
            return html;
        }


        public StringBuilder ToHTMLBody()
        {
            var html = new StringBuilder();

            html.Append("		<center style=\"text-align:center\">");
            html.Append("			<h2>ОГОЛОШЕННЯ</h2>");
            html.Append("			<br>");
            //html.Append($"			<div><a href=\"https://zakupivli24.pb.ua/prozorro/tender/{this.TenderID}\">{this.TenderID}</a></div>");
            html.Append($"          <div id=\"tender-info\" data-id=\"{this.Id}\" data-tenderid=\"{this.TenderID}\" data-title=\"{this.Title.Replace("\"", "\\\"")}\" data-procuringentity-name=\"{this.ProcuringEntity?.Name.Replace("\"", "\\\"")}\">");
            html.Append($"			    <span id=\"tender-tenderid\">{this.TenderID}</span>");
            html.Append($"	            | <a  href=\"https://prozorro.gov.ua/tender/{this.TenderID}\">на Prozorro</a> | <a href=\"https://dozorro.org/tender/{this.TenderID}\">на Dozorro</a> | <a href=\"https://zakupivli24.pb.ua/prozorro/tender/{this.TenderID}\">на Zakupivli24</a>");
            html.Append("           </div>");

            html.Append("		</center>");
            html.Append("		<br><br>");
            html.Append("		<table cellpadding=\"5\" cellspacing=\"0\" border=\"0\" width=\"100%\">");
            html.Append("		<tr>");
            html.Append("			<td width=\"30\">1.</td>");
            html.Append("			<td width=\"272\">Найменування замовника:</td>");
            html.Append($"			<td><div id=\"tender-procuringentity-name\" style=\"font-weight: bold;\">{this.ProcuringEntity?.Name}</div></td>");
            html.Append("		</tr>");
            html.Append("		<tr>");
            html.Append("			<td>2.</td>");
            html.Append("			<td>Код згідно з ЄДРПОУ замовника:</td>");
            //html.Append($"			<td><strong><a href=\"https://zakupivli24.pb.ua/prozorro/company/{this.ProcuringEntity?.Identifier?.Scheme}/{this.ProcuringEntity?.Identifier?.Id}\">{this.ProcuringEntity?.Identifier?.Id}</a></strong></td>");
            html.Append($"			<td><strong><a href=\"https://dozorro.org/tender/search/?edrpou={this.ProcuringEntity?.Identifier?.Id}\">{this.ProcuringEntity?.Identifier?.Id}</a></strong></td>");
            html.Append("		</tr>");
            html.Append("		<tr>");
            html.Append("			<td>3.</td>");
            html.Append("			<td>Місцезнаходження замовника:</td>");
            html.Append($"		<td><strong>{this.ProcuringEntity?.Address?.PostalCode}, {this.ProcuringEntity?.Address?.CountryName}, {this.ProcuringEntity?.Address?.Region}, {this.ProcuringEntity?.Address?.Locality}, {this.ProcuringEntity?.Address?.StreetAddress}</strong></td>");
            html.Append("		</tr>");
            html.Append("		<tr>");
            html.Append("			<td>4.</td>");
            html.Append("			<td>Контактна особа замовника, уповноважена здійснювати зв’язок з учасниками:</td>");
            html.Append($"			<td><strong>{this.ProcuringEntity?.ContactPoint?.Name}, {this.ProcuringEntity?.ContactPoint?.Telephone}, {this.ProcuringEntity?.ContactPoint?.Email}</strong></td>");
            html.Append("		</tr>");
            html.Append("		</table>");

            html.Append("		<style>");
            html.Append("			strong span{");
            html.Append("				margin-left:5px;");
            html.Append("			}");
            html.Append("		</style>");


            html.Append("		<table cellpadding=\"5\" cellspacing=\"1\" border=\"0\" width=\"100%\" class=\"border\">");
            html.Append("			<tr valign=\"top\">");
            html.Append("				<td>5. Конкретна назва предмета закупівлі</td>");
            html.Append("				<td>6. Коди відповідних класифікаторів предмета закупівлі (за наявності)</td>");
            html.Append("				<td>7. Кількість товарів або обсяг виконання робіт чи надання послуг</td>");
            html.Append("				<td>8. Місце поставки товарів або місце виконання робіт чи надання послуг</td>");
            html.Append("				<td>9. Строк поставки товарів, виконання робіт чи надання послуг</td>");
            html.Append("			</tr>");

            foreach (Item item in this.Items)
            {
                html.Append("			<tr valign=\"top\">");
                html.Append("				<td>");
                html.Append($"					<div>{item.Description}</div>");
                html.Append("				</td>");
                html.Append("				<td>");
                html.Append($"					<div>{item.Classification?.Scheme}: {item.Classification?.Id} — {item.Classification?.Description}</div>");
                html.Append("				</td>");
                html.Append("				<td>");
                html.Append($"					{item.Quantity} {item.Unit.Name}");
                html.Append("				</td>");
                html.Append("				<td>");
                html.Append($"					{item.DeliveryAddress?.PostalCode}, {item.DeliveryAddress?.CountryName}, {item.DeliveryAddress?.Region}, {item.DeliveryAddress?.Locality}, {item.DeliveryAddress?.StreetAddress}");
                html.Append("				</td>");
                html.Append("				<td class=\"small\">");
                html.Append($"					до {item.DeliveryDate?.EndDate} ");
                html.Append("				</td>");
                html.Append("			</tr>");
            }

            html.Append("		</table>");

            return html;
        }

    }
}
