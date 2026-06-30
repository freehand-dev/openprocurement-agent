using System.Text;
using openprocurement.api.client.Models;

namespace openprocurement_agent
{
    public static class TenderHtmlExtensions
    {
        public static StringBuilder ToHtml(this Tender t)
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

            html.Append(t.ToHtmlBody());

            html.Append("	</body>");
            html.Append("</html>");
            return html;
        }

        public static StringBuilder ToHtmlBody(this Tender t)
        {
            var html = new StringBuilder();

            html.Append("		<center style=\"text-align:center\">");
            html.Append("			<h2>ОГОЛОШЕННЯ</h2>");
            html.Append("			<br>");
            //html.Append($"			<div><a href=\"https://zakupivli24.pb.ua/prozorro/tender/{this.TenderID}\">{this.TenderID}</a></div>");
            html.Append($"          <div id=\"tender-info\" data-id=\"{t.Id}\" data-tenderid=\"{t.TenderID}\" data-title=\"{t.Title.Replace("\"", "\\\"")}\" data-procuringentity-name=\"{t.ProcuringEntity?.Name.Replace("\"", "\\\"")}\">");
            html.Append($"			    <span id=\"tender-tenderid\">{t.TenderID}</span>");
            html.Append($"	            | <a  href=\"https://prozorro.gov.ua/tender/{t.TenderID}\">на Prozorro</a> | <a href=\"https://dozorro.org/tender/{t.TenderID}\">на Dozorro</a> | <a href=\"https://zakupivli24.pb.ua/prozorro/tender/{t.TenderID}\">на Zakupivli24</a>");
            html.Append("           </div>");

            html.Append("		</center>");
            html.Append("		<br><br>");
            html.Append("		<table cellpadding=\"5\" cellspacing=\"0\" border=\"0\" width=\"100%\">");
            html.Append("		<tr>");
            html.Append("			<td width=\"30\">1.</td>");
            html.Append("			<td width=\"272\">Найменування замовника:</td>");
            html.Append($"			<td><div id=\"tender-procuringentity-name\" style=\"font-weight: bold;\">{t.ProcuringEntity?.Name}</div></td>");
            html.Append("		</tr>");
            html.Append("		<tr>");
            html.Append("			<td>2.</td>");
            html.Append("			<td>Код згідно з ЄДРПОУ замовника:</td>");
            //html.Append($"			<td><strong><a href=\"https://zakupivli24.pb.ua/prozorro/company/{this.ProcuringEntity?.Identifier?.Scheme}/{this.ProcuringEntity?.Identifier?.Id}\">{this.ProcuringEntity?.Identifier?.Id}</a></strong></td>");
            html.Append($"			<td><strong><a href=\"https://dozorro.org/tender/search/?edrpou={t.ProcuringEntity?.Identifier?.Id}\">{t.ProcuringEntity?.Identifier?.Id}</a></strong></td>");
            html.Append("		</tr>");
            html.Append("		<tr>");
            html.Append("			<td>3.</td>");
            html.Append("			<td>Місцезнаходження замовника:</td>");
            html.Append($"		<td><strong>{t.ProcuringEntity?.Address?.PostalCode}, {t.ProcuringEntity?.Address?.CountryName}, {t.ProcuringEntity?.Address?.Region}, {t.ProcuringEntity?.Address?.Locality}, {t.ProcuringEntity?.Address?.StreetAddress}</strong></td>");
            html.Append("		</tr>");
            html.Append("		<tr>");
            html.Append("			<td>4.</td>");
            html.Append("			<td>Контактна особа замовника, уповноважена здійснювати зв’язок з учасниками:</td>");
            html.Append($"			<td><strong>{t.ProcuringEntity?.ContactPoint?.Name}, {t.ProcuringEntity?.ContactPoint?.Telephone}, {t.ProcuringEntity?.ContactPoint?.Email}</strong></td>");
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

            foreach (Item item in t.Items)
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