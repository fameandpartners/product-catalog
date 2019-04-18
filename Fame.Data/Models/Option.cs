using System;
using System.Collections.Generic;

namespace Fame.Data.Models
{
    [Serializable]
    public class Option
    {
        public int OptionId { get; set; }

        public string Title { get; set; }
        
        public string ComponentId { get; set; }

        public int ProductVersionId { get; set; }

        public Component Component { get; set; }
        
        public ProductVersion ProductVersion { get; set; }
        
        public ICollection<CompatibleOption> CompatibleOptions { get; set; }
        
        public ICollection<IncompatibleOption> IncompatibleOptions { get; set; }
        
        public ICollection<OptionPrice> Prices { get; set; }

        public ICollection<OptionRenderComponent> OptionRenderComponents { get; set; }

        public ICollection<Incompatibility> IncompatibleParentOptions { get; set; }

        public ICollection<Incompatibility> Incompatibilities { get; set; }
    }
}
