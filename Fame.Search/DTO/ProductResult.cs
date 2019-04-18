using System;
using System.Collections.Generic;
using Fame.Service.DTO;

namespace Fame.Search.DTO
{
    [Serializable]
    public class ProductResult
    {
        public List<ProductSummary> Results { get; set; }

        public Dictionary<string, List<FacetCategorySummary>> FacetConfigurations { get; set; }
        
        public Dictionary<string, FacetGroupSummary> FacetGroups { get; set; }

        public int? LastIndex { get; set; }

        public decimal? LastValue { get; set; }
        
        public bool HasMore { get; set; }
    }
}
