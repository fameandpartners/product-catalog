using Fame.Data.Models;
using System;
using System.Collections.Generic;

namespace Fame.Service.DTO
{
    [Serializable]
    public class SectionGroupSummary
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string AggregateTitle { get; set; }
        public PreviewType PreviewType { get; set; }
        public string RenderPositionId { get; set; }
        public List<SectionSummary> Sections { get; set; }
    }
}