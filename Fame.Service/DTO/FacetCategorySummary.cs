using System;
using System.Collections.Generic;

namespace Fame.Service.DTO
{
    [Serializable]
    public class FacetCategorySummary
    {
        public string Name { get; set; }
        public bool HideHeader { get; set; }

        public List<string> FacetGroupIds { get; set; } 
    }
}