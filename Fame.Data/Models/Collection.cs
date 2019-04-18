using System.Collections.Generic;

namespace Fame.Data.Models
{
    public class Collection
    {
        public string CollectionId { get; set; }
        public string CollectionName { get; set; }

        public ICollection<CollectionProduct> Products { get; set; }
        public ICollection<CollectionFacet> Facets { get; set; }
        public ICollection<CollectionOccasion> Occasions { get; set; }
        public ICollection<CollectionFacetBoost> FacetBoosts { get; set; }
    }

    public class CollectionProduct
    {
        public string ProductId { get; set; }
        public string CollectionId { get; set; }

        public Product Product { get; set; }
        public Collection Collection { get; set; }
    }

    public class CollectionFacet
    {
        public string FacetId { get; set; }
        public string CollectionId { get; set; }

        public Facet Facet { get; set; }
        public Collection Collection { get; set; }
    }

    public class CollectionOccasion
    {
        public string OccasionId { get; set; }
        public string CollectionId { get; set; }

        public Occasion Occasion { get; set; }
        public Collection Collection { get; set; }
    }

    public class CollectionFacetBoost
    {
        public int FacetBoostId { get; set; }
        public string CollectionId { get; set; }

        public FacetBoost FacetBoost { get; set; }
        public Collection Collection { get; set; }
    }
}
