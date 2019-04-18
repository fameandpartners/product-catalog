using System;

namespace Fame.Data.Models
{
    [Serializable]
    public class FacetCategoryGroup
    {
        public string FacetGroupId { get; set; }
        public string FacetCategoryId { get; set; }

        public FacetGroup FacetGroup { get; set; }
        public FacetCategory FacetCategory { get; set; }
    }
}