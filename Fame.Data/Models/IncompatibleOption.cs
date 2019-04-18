using System;

namespace Fame.Data.Models
{
    [Serializable]
    public class IncompatibleOption
    {
        public int IncompatibilityId { get; set; }
     
        public int OptionId { get; set; }

        public Incompatibility Incompatibility { get; set; }
        
        public Option Option { get; set; }
    }
}
