using System;

namespace Fame.Data.Models
{
    [Serializable]
    public class OptionPrice
    {
        public int OptionId { get; set; }
        public string LocalisationCode { get; set; }
        public int PriceInMinorUnits { get; set; }

        public Option Option { get; set; }
    }
}