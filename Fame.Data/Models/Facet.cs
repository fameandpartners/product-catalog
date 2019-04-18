using System;
using System.Collections.Generic;

namespace Fame.Data.Models
{
    [Serializable]
    public class Facet
    {
        public string FacetId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string FacetGroupId { get; set; }
        public int Order { get; set; }
        public string CompatibilityRule { get; set; }
        public int TagPriority { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PreviewImage { get; set; }
        public string TaxonString { get; set; }

        public ICollection<FacetMeta> FacetMeta { get; set; }
        public ICollection<Curation> Curations { get; set; }
        public FacetGroup FacetGroup { get; set; }
        public ICollection<CollectionFacet> Collections { get; set; }

        public string[] Taxons => string.IsNullOrEmpty(TaxonString) ? new string[] { } : TaxonString.Split("|");
    }
}