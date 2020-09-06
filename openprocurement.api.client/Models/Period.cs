using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class Period
    {
        /// <summary>
        /// OpenContracting Description: The start date for the period.
        /// </summary>
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// OpenContracting Description: The end date for the period.
        /// </summary>
        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }

    }
}
