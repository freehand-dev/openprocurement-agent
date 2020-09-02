using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class PeriodDate
    {
        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("shouldStartAfter")]
        public DateTime? ShouldStartAfter { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }

    }
}
