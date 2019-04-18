using System;
using System.Collections.Generic;

namespace Fame.Service.DTO
{
    [Serializable]
    public class SectionSummary
    {
        public string ComponentTypeId { get; set; }
        public string ComponentTypeCategory { get; set; }
        public string Title { get; set; }
        public string SelectionType { get; set; }
        public List<CompatibleOptionSummary> Options { get; set; }
    }
}