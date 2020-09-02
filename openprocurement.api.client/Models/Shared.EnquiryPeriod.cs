using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class EnquiryPeriod: PeriodDate
    {
        [JsonPropertyName("clarificationsUntil")]
        public DateTime ClarificationsUntil { get; set; }

        [JsonPropertyName("invalidationDate")]
        public DateTime InvalidationDate { get; set; }
    }
}
