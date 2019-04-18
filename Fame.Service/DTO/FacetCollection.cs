using System.Collections.Generic;
using Fame.Data.Models;

namespace Fame.Service.DTO
{
    public class FacetCollection
    {
        public Facet Facet { get; set; }
        public List<List<string>> ComponentCollections { get; set; }
    }
}