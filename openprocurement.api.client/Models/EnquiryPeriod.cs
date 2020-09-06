using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class EnquiryPeriod: Period
    {
        /// <summary>
        /// date of the last tender conditions modification, when all bid proposals became invalid. Broker (eMall) should take action in order for bids to be activated or re-submitted
        /// </summary>
        [JsonPropertyName("clarificationsUntil")]
        public DateTime ClarificationsUntil { get; set; }

        /// <summary>
        /// time before which answers for questions and claims can be provided. After this time the procedure will be blocked.
        /// </summary>
        [JsonPropertyName("invalidationDate")]
        public DateTime InvalidationDate { get; set; }
    }
}
