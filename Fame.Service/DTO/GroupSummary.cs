using System;
using System.Collections.Generic;

namespace Fame.Service.DTO
{
    [Serializable]
    public class GroupSummary
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public List<SectionGroupSummary> SectionGroups { get; set; }
        public string SelectionTitle { get; set; }
        public bool Hidden { get; internal set; }
    }
}