using System;

namespace Fame.Data.Models
{
    [Serializable]
    public class ProductVersionPrice
    {
        public int ProductVersionId { get; set; }
        public string LocalisationCode { get; set; }
        public int PriceInMinorUnits { get; set; }

        public ProductVersion ProductVersion { get; set; }
    }
}