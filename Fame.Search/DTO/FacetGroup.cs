using System.Collections.Generic;

namespace Fame.Search.DTO
{
    public class FacetGroup
    {
        public string FacetGroupId { get; set; }
        public List<FacetCount> FacetCounts { get; set; }
    }
}