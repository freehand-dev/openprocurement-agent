using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class TendersResponse: DataResponse<List<TenderBase>>
    {
        [JsonPropertyName("next_page")]
        public NavigationPage NextPage { get; set; }

        [JsonPropertyName("prev_page")]
        public NavigationPage PrevPage { get; set; }

    }
}
