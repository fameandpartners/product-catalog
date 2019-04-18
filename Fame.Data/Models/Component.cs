using System;
using System.Collections.Generic;

namespace Fame.Data.Models
{
    [Serializable]
    public class Component
    {
        public string ComponentId { get; set; }

        public string Title { get; set; }

        public string CartId { get; set; }

        public string ComponentTypeId { get; set; }

        public string RenderPositionId { get; set; }

        public string ManufacturingSortOrderId { get; set; }

        public bool IsolateInSummary { get; set; }

        public Zoom? PreviewZoom { get; set; }

        public ManufacturingSortOrder ManufacturingSortOrder { get; set; }

        public RenderPosition RenderPosition { get; set; }

        public ComponentType ComponentType { get; set; }

        public ICollection<ComponentMeta> ComponentMeta { get; set; }

        public int Sort { get; set; }
        
        public bool Indexed { get; set; }

        public List<CurationComponent> CurationComponents { get; set; }
    }
}
