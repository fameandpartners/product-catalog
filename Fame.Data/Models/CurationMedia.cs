using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Fame.Data.Models
{
    [Serializable]
	public class CurationMedia
    {
        public int Id { get; set; }
        public MediaType Type { get; set; }
        public string PID { get; set; }
        public int SortOrder { get; set; }
        public int PLPSortOrder { get; set; }
        public string FitDescription { get; set; }
        public string SizeDescription { get; set; }
        public DateTime? LastModified { get; set; }
        public bool Archived { get; set; }
        
        public Curation Curation { get; set; }
        public List<CurationMediaVariant> CurationMediaVariants { get; set; }
    }

    public enum MediaType
    {
        [EnumMember(Value = "render")] Render,
        [EnumMember(Value = "image")] Image,  // CADs in legacy system
        [EnumMember(Value = "photo")] Photo   // On Body shots
    }
}