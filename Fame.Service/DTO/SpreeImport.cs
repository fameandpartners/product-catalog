using System;
using System.Collections.Generic;
using Fame.Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fame.Service.DTO
{
    public class SpreeImport
    {
        [JsonProperty("details")]
        public SpreeImport.ProductDetails Details { get; set; }

        [JsonProperty("style_number")]
        public string StyleNumber { get; set; }

        [JsonProperty("customization_list")]
        public IEnumerable<SpreeImport.Customization> CustomizationList { get; set; }

        [JsonProperty("making_options")]
        public IEnumerable<SpreeImport.Customization> MakingOptions { get; set; }

        [JsonProperty("curations")]
        public IEnumerable<SpreeImport.Curation> Curations { get; set; }

        public class Curation
        {
            [JsonProperty("pid")]
            public string Pid { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("taxons")]
            public IEnumerable<string> Taxons { get; set; }

            [JsonProperty("media")]
            public IEnumerable<string> Media { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }
        }

        public class Customization
        {
            [JsonProperty("presentation")]
            public string CustomizationPresentation { get; set; }

            [JsonProperty("customization_id")]
            public string CustomizationId { get; set; }

            [JsonProperty("price_usd")]
            public string PriceUsd { get; set; }

            [JsonProperty("price_aud")]
            public string PriceAud { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("hex")]
            public String Hex { get; set; }

            [JsonProperty("manifacturing_sort_order")]
            public int ManifacturingSortOrder { get; set; }
        }

        public class ProductDetails
        {
            [JsonProperty("active")]
            public Boolean Active { get; set; }

            [JsonProperty("fabrics")]
            public IEnumerable<SpreeImport.Customization> Fabrics { get; set; }

            [JsonProperty("colors")]
            public IEnumerable<SpreeImport.Customization> Colors { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("style_notes")]
            public string StyleNotes { get; set; }

            [JsonProperty("fit")]
            public string Fit { get; set; }

            [JsonProperty("fabric")]
            public string Fabric { get; set; }

            [JsonProperty("short_description")]
            public string ShortDescription { get; set; }

            [JsonProperty("factory_name")]
            public string Factory { get; set; }

            [JsonProperty("primary_image")]
            public string PrimaryImage { get; set; }

            [JsonProperty("secondary_images")]
            public IEnumerable<string> SecondaryImages { get; set; }

            [JsonProperty("taxons")]
            public IEnumerable<string> Taxons { get; set; }

            [JsonProperty("type")]
            [JsonConverter(typeof(StringEnumConverter))]
            public ProductType Type { get; set; }

            [JsonProperty("price_usd")]
            public string PriceUsd { get; set; }

            [JsonProperty("price_aud")]
            public string PriceAud { get; set; }
        }
    }

}