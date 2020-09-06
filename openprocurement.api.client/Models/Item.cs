using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.Models
{
    public class Item
    {
        /// <summary>
        /// auto-generated
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// OpenContracting Description: A description of the goods, services to be provided.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// OpenContracting Description: The primary classification for the item. See the itemClassificationScheme to identify preferred classification lists, including CPV and GSIN.
        /// It is mandatory for classification.scheme to be CPV or ДК021. The classification.id should be valid CPV or ДК021 code.
        /// </summary>
        [JsonPropertyName("classification")]
        public Classification Classification { get; set; }

        /// <summary>
        /// OpenContracting Description: An array of additional classifications for the item. See the itemClassificationScheme codelist for common options to use in OCDS. This may also be used to present codes from an internal classification scheme.
        /// </summary>
        [JsonPropertyName("additionalClassifications")]
        public List<Classification> AdditionalClassifications { get; set; }

        /// <summary>
        /// OpenContracting Description: Description of the unit which the good comes in e.g. hours, kilograms. Made up of a unit name, and the value of a single unit.
        /// </summary>
        [JsonPropertyName("unit")]
        public Unit Unit { get; set; }

        /// <summary>
        /// OpenContracting Description: The number of units required
        /// </summary>
        [JsonPropertyName("quantity")]
        public float Quantity { get; set; }

        /// <summary>
        /// Address, where the item should be delivered.
        /// </summary>
        [JsonPropertyName("deliveryAddress")]
        public Address DeliveryAddress { get; set; }

        /// <summary>
        /// Period during which the item should be delivered.
        /// </summary>
        [JsonPropertyName("deliveryDate")]
        public Period DeliveryDate { get; set; }

        /// <summary>
        /// Geographical coordinates of delivery location.
        /// </summary>
        [JsonPropertyName("deliveryLocation")]
        public Location DeliveryLocation { get; set; }

        /// <summary>
        /// Id of related Lot.
        /// </summary>
        [JsonPropertyName("relatedLot")]
        public string RelatedLot { get; set; }

    }
}
