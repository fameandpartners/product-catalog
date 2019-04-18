using System;
using System.Collections.Generic;

namespace Fame.Data.Models
{
    [Serializable]
    public class Incompatibility
    {
        public int IncompatibilityId { get; set; }

        public int OptionId { get; set; }

        public int? ParentOptionId { get; set; }
        
        public Option ParentOption { get; set; }
        
        public Option Option { get; set; }
        
        public ICollection<IncompatibleOption> IncompatibleOptions { get; set; }
    }
}