using System.Collections.Generic;

namespace Fame.Data.Models
{
    public class Occasion
    {
        public string OccasionId { get; set; }
        public string OccasionName { get; set; }
        public string ComponentCompatibilityRule { get; set; }
        public string FacetCompatibilityRule { get; set; }

        public ICollection<CollectionOccasion> Collections { get; set; }
    }
}
