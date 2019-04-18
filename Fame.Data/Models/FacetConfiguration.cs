using System;
using System.Collections.Generic;

namespace Fame.Data.Models
{
    [Serializable]
    public class FacetConfiguration
    {
        public string FacetConfigurationId { get; set; }
        public string Title { get; set; }

        public ICollection<FacetCategoryConfiguration> FacetCategoryConfigurations { get; set; }
    }
}