using System.Collections.Generic;

namespace Fame.Data.Models
{
    [System.Serializable]
    public class ComponentType
    {
        public string ComponentTypeId { get; set; }

        public string Title { get; set; } // e.g. top

        public string SelectionTitle { get; set; } // e.g. Select your top

        public decimal SortWeightDefault { get; set; }

        public decimal SortWeightOther { get; set; }

        /// <summary>
        /// Used to identify whether the component code is used as part of the product url
        /// </summary>
        public bool IsProductCode { get; set; }

        /// <summary>
        /// Any components of this type should be aggregated when added to search.
        /// </summary>
        public bool AggregateOnIndex { get; set; }

        public ComponentTypeCategory ComponentTypeCategory { get; set; }

        public string ParentComponentTypeId { get; set; }

        public ComponentType ParentComponentType { get; set; }

        public ICollection<ComponentType> ChildComponentTypes { get; set; }

        public ICollection<ProductRenderComponent> ProductRenderComponents { get; set; }

        public ICollection<OptionRenderComponent> OptionRenderComponents { get; set; }

        public ICollection<Component> Components { get; set; }

        /// <summary>
        /// Sections with this component type
        /// </summary>
        public ICollection<Section> Sections { get; set; }
    }
}
