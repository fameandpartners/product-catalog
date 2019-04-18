using System;
using System.Collections.Generic;

namespace Fame.Data.Models
{
    [Serializable]
    public class FacetCategory
    {
        public string FacetCategoryId { get; set; }
        public string Title { get; set; }
        public int Sort { get; set; }
        public bool HideHeader { get; set; }
        
        public ICollection<FacetCategoryGroup> FacetCategoryGroups { get; set; }
        public ICollection<FacetCategoryConfiguration> FacetCategoryConfigurations { get; set; }
    }
}