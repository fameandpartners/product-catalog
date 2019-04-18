using System;
using System.Collections.Generic;

namespace Fame.Data.Models
{
    [Serializable]
    public class Group
    {
        public int GroupId { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public int Order { get; set; }
        public bool Hidden { get; set; }

        public int ProductVersionId { get; set; }

        public ProductVersion ProductVersion { get; set; }

        public ICollection<SectionGroup> SectionGroups { get; set; }
        
        public string SelectionTitle { get; set; }
    }
}
