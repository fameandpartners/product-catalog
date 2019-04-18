using System;
using System.Collections.Generic;

namespace Fame.Data.Models
{
    [Serializable]
    public class FacetGroup
    {
        public string FacetGroupId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int Sort { get; set; }
        public bool IsAggregatedFacet { get; set; }
        public bool IsCategoryFacet { get; set; }
        public bool Multiselect { get; set; }
        public bool Collapsed { get; set; }
        public string Slug { get; set; }
        public int? ProductNameOrder { get; set; }
        public bool PrimarySilhouette { get; set; }

        public ICollection<FacetCategoryGroup> FacetCategoryGroups { get; set; }

        public ICollection<Facet> Facets { get; set; }
    }
}
