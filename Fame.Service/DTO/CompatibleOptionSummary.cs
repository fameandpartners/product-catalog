using System;

namespace Fame.Service.DTO
{
    [Serializable]
    public class CompatibleOptionSummary
    {
        public string Code { get; set; }
        public bool IsDefault { get; set; }
        public string ParentOptionId { get; set; }
    }
}