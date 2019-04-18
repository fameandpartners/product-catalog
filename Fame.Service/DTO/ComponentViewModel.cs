using System;
using Fame.Data.Models;

namespace Fame.Service.DTO
{
    [Serializable]
    public class ComponentViewModel
    {
        public string ComponentId { get; set; }
        public bool IsolateInSummary { get; set; }
        public string ComponentTypeId { get; set; }
        public Zoom? PreviewZoom { get; internal set; }
        public string Title { get; internal set; }
    }
}
