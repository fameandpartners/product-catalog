using System.Collections.Generic;

namespace Fame.Data.Models
{
    [System.Serializable]
    public class Section
    {
        public int SectionId { get; set; }

        public string Title { get; set; }

        public SelectionType SelectionType { get; set; }
       
        public int SectionGroupId { get; set; }

        public string ComponentTypeId { get; set; }

        public int Order { get; set; }

        /// <summary>
        /// The component type that all options are of
        /// </summary>
        public ComponentType ComponentType  { get; set; }

        public SectionGroup SectionGroup { get; set; }

        public ICollection<CompatibleOption> CompatibleOptions { get; set; }
    }

    public enum SelectionType
    {
        RequiredOne,
        OptionalOne,
        OptionalMultiple
    }
}
