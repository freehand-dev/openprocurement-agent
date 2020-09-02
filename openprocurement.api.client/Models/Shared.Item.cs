using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class TenderItem
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("classification")]
        public Classification Classification { get; set; }

        [JsonPropertyName("deliveryAddress")]
        public Address DeliveryAddress { get; set; }

        [JsonPropertyName("deliveryDate")]
        public PeriodDate DeliveryDate { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("unit")]
        public Unit Unit { get; set; }

        [JsonPropertyName("quantity")]
        public float Quantity { get; set; }

        
    }
}
