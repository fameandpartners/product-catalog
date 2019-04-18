using System;
using System.Collections.Generic;

namespace Fame.Data.Models
{
    [Serializable]
    public class ProductVersion
    {
        public int ProductVersionId { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public string Factory { get; set; }
        
        public VersionState VersionState { get; set; }

        public string ProductId { get; set; }
        
        public Product Product { get; set; }

        public ICollection<Option> Options { get; set; }

        public ICollection<Group> Groups { get; set; }

        public ICollection<ProductRenderComponent> ProductRenderComponents { get; set; }

        public ICollection<ProductVersionPrice> Prices { get; set; }
    }
}
