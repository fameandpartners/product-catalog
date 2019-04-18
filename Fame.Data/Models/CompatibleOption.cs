namespace Fame.Data.Models
{
    [System.Serializable]
    public class CompatibleOption
    {
        public int CompatibleOptionId { get; set; } // Necessary because EF won't allow composite keys using nullable values.

        public int SectionId { get; set; } // Unique index added across SectionId, OptionId & ParentOptionId
        public int OptionId { get; set; }
        public int? ParentOptionId { get; set; }

        public bool IsDefault { get; set; }

        public Section Section { get; set; }
        public Option Option { get; set; }
        public Option ParentOption { get; set; }
    }
}