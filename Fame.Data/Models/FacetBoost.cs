using System;
using System.Collections.Generic;

namespace Fame.Data.Models
{
    [Serializable]
    public class FacetBoost
    {
        public int FacetBoostId { get; set; }
        public string BoostRule { get; set; }
        public decimal BoostWeight { get; set; }

        public ICollection<CollectionFacetBoost> Collections { get; set; }
    }
}