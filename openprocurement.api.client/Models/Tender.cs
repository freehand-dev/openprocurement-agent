using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace openprocurement.api.client.Models
{
    public class Tender: TenderBase
    {
        [JsonPropertyName("description_en")]
        public string DescriptionEng { get; set; }

        [JsonPropertyName("title_en")]
        public string TitleEng { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("procurementMethod")]
        public string ProcurementMethod { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("buyers")]
        public List<Buyer> Buyers { get; set; }

        [JsonPropertyName("documents")]
        public List<TenderDocument> Documents { get; set; }

        [JsonPropertyName("mainProcurementCategory")]
        public string MainProcurementCategory { get; set; }

        [JsonPropertyName("complaintPeriod")]
        public PeriodDate ComplaintPeriod { get; set; }

        [JsonPropertyName("auctionPeriod")]
        public PeriodDate AuctionPeriod { get; set; }
        
        [JsonPropertyName("enquiryPeriod")]
        public EnquiryPeriod EnquiryPeriod { get; set; }

        [JsonPropertyName("submissionMethod")]
        public string SubmissionMethod { get; set; }

        [JsonPropertyName("next_check")]
        public DateTime NextCheck { get; set; }

        [JsonPropertyName("awardCriteria")]
        public string AwardCriteria { get; set; }

        [JsonPropertyName("tenderPeriod")]
        public PeriodDate TenderPeriod { get; set; }

        [JsonPropertyName("plans")]
        public List<Plan> Plans { get; set; }

        [JsonPropertyName("contracts")]
        public List<TenderContract> Contracts { get; set; }

        [JsonPropertyName("minimalStep")]
        public TenderValue MinimalStep { get; set; }
        
        [JsonPropertyName("items")]
        public List<TenderItem> Items { get; set; }

        [JsonPropertyName("procurementMethodType")]
        public string ProcurementMethodType { get; set; }

        [JsonPropertyName("value")]
        public TenderValue Value { get; set; }

        [JsonPropertyName("owner")]
        public string Owner { get; set; }

        [JsonPropertyName("awards")]
        public List<TenderAward> Awards { get; set; }

        [JsonPropertyName("tenderID")]
        public string TenderID { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("procuringEntity")]
        public TenderProcuringEntity ProcuringEntity { get; set; }

        [JsonPropertyName("milestones")]
        public List<Milestone> Milestones { get; set; }

        public StringBuilder ToHTML()
        {
            var html = new StringBuilder();

            html.Append("<html>");
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

            html.Append("		<center style=\"text-align:center\">");
            html.Append("			<h2>ОГОЛОШЕННЯ</h2>");
            html.Append($"			<div>про проведення спрощеної/допорогової закупівлі<br><a href=\"https://prozorro.gov.ua/tender/{this.TenderID}\">{this.TenderID}</a></div>");
            html.Append("		</center>");
            html.Append("		<br><br>");
            html.Append("		<table cellpadding=\"5\" cellspacing=\"0\" border=\"0\" width=\"100%\">");
            html.Append("		<tr>");
            html.Append("			<td width=\"30\">1.</td>");
            html.Append("			<td width=\"272\">Найменування замовника:</td>");
            html.Append($"			<td><strong>{this.ProcuringEntity?.Name}</strong></td>");
            html.Append("		</tr>");
            html.Append("		<tr>");
            html.Append("			<td>2.</td>");
            html.Append("			<td>Код згідно з ЄДРПОУ замовника:</td>");
            html.Append($"			<td><strong>{this.ProcuringEntity?.Identifier?.Id}</strong></td>");
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

            foreach (TenderItem item in this.Items)
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


            html.Append("	</body>");
            html.Append("</html>");
            return html;
        }

    }
}
