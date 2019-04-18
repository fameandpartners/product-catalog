using System;
using Fame.Data.Models;

namespace Fame.Service.DTO
{
    [Serializable]
    public class RenderPositionSummary
    {
        public string RenderPositionId { get; set; }
        public Zoom Zoom { get; set; } 
        public Orientation Orientation { get; set; }
    }
}