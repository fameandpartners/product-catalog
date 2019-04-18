using System;
using System.Collections.Generic;

namespace Fame.Service.DTO
{
    [Serializable]
    public class ComponentSummary
    {
        public string CartId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string ComponentTypeId { get; set; }
        public decimal Price { get; set; }
        public bool IsProductCode { get; set; }
        public bool IsRecommended { get; set; }
        public string ComponentTypeCategory { get; set; }
        public List<IncompatibleWith> IncompatibleWith { get; set; }
        public List<string> OptionRenderComponents { get; set; }
        public string RenderPositionId { get; set; }
        public Dictionary<string, string> Meta { get; set; }
        public int SortOrder { get; set; }
    }
}