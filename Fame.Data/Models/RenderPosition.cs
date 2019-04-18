using System;
using System.Collections.Generic;

namespace Fame.Data.Models
{
    [Serializable]
    public class RenderPosition
    {
        public string RenderPositionId { get; set; }
        public Zoom Zoom { get; set; } 
        public Orientation Orientation { get; set; }

        public ICollection<Component> Components { get; set; }
        public ICollection<SectionGroup> SectionGroups { get; set; }
    }
}