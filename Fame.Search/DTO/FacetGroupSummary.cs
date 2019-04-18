using System;
using System.Collections.Generic;

namespace Fame.Search.DTO
{
    [Serializable]
    public class FacetGroupSummary
    {
        public string GroupId { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public List<FacetCount> Facets { get; set; }
        
        public bool IsAggregated { get; set; }

        public bool IsCategoryFacet { get; set; }

        public bool Multiselect { get; set; }

        public bool Collapsed { get; set; }

        public string Slug { get; set; }
    }
}