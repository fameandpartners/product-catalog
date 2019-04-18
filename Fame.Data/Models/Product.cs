using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Fame.Data.Models
{
    [Serializable]
    public class Product
    {
        public string ProductId { get; set; }

        public string Title { get; set; }

        public bool Index { get; set; }

        public ProductType ProductType { get; set; }

        public PreviewType PreviewType { get; set; }

        public ICollection<ProductVersion> ProductVersion { get; set; }

        public ICollection<Curation> Curations { get; set; }

        public ICollection<CollectionProduct> Collections { get; set; }

        public string DropBoxAssetFolder { get; set; }

        public string DropName { get; set; }

        public bool DisableLayering { get; set; }
    }

    [Serializable]
    public enum ProductType
    {
        [EnumMember(Value = "dress")] Dress,
        [EnumMember(Value = "swatch")] Swatch,
    }

    [Serializable]
    public enum PreviewType
    {
        [EnumMember(Value = "render")] Render,
        [EnumMember(Value = "cad")] Cad
    }
}
