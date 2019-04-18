using System.Collections.Generic;

namespace Fame.Service.DTO
{
    public class GroupedFacet
    {
        public string GroupId { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public List<FacetSummary> Facets { get; set; }

        public bool IsAggregated { get; set; }

        public bool IsCategoryFacet { get; set; }

        public bool Multiselect { get; set; }

        public bool Collapsed { get; set; }

        public string Slug { get; set; }

        public string Name { get; set; }
    }
}
