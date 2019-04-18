using System;

namespace Fame.Data.Models
{
    [Serializable]
    public class FacetCategoryConfiguration 
    {
        public string FacetCategoryId { get; set; }
        public string FacetConfigurationId { get; set; }
        
        public FacetCategory FacetCategory { get; set; }
        public FacetConfiguration FacetConfiguration { get; set; }
    }
}