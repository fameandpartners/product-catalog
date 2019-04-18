using System.Collections.Generic;
using System.Linq;
using Fame.Service.DTO;

namespace Fame.Service.Extensions
{
    public static class GroupedFacetListExtension
    {
        public static List<string> Selected(this IEnumerable<GroupedFacet> list)
        {
            return list.SelectMany(fg => fg.Facets.Where(f => f.Selected)).Select(f => f.FacetId).ToList();
        }
    }
}
