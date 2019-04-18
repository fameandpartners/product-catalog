using Fame.Data.Models;
using System.Linq;

namespace Fame.Service.Extensions
{
    public static class CurationExtensions
    {
        public static string ToRenderUrl(this Curation curation, string renderUrlPrefix)
        {
            var curationIds = curation.CurationComponents.Select(c => c.ComponentId).OrderBy(c => c);
            return $@"{renderUrlPrefix}/{curation.ProductId}/{Orientation.Front}{(Zoom.None)}/{Size.SIZE_OPTION_352}/{string.Join('~', curationIds)}.png";
        }
    }
}
