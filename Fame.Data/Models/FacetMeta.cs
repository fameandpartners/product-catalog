using System;

namespace Fame.Data.Models
{
    [Serializable]
    public class FacetMeta
    {
        public string FacetId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public Facet Facet { get; set; }
    }
}