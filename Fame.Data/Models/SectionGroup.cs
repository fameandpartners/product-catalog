using System.Collections.Generic;

namespace Fame.Data.Models
{
    [System.Serializable]
    public class SectionGroup
    {
        public int SectionGroupId { get; set; }

        public string Title { get; set; }

        public int GroupId { get; set; }

        public string Slug { get; set; }

        public string AggregateTitle { get; set; }

        public int Order { get; set; }

        public string RenderPositionId { get; set; }

        public RenderPosition RenderPosition { get; set; }

        public Group Group { get; set; }

        public ICollection<Section> Sections { get; set; }
    }
}
