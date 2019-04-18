using Fame.Data.Models;

namespace Fame.Service.Services
{
    public interface IFacetGroupService : IBaseService<FacetGroup>
    {
        FacetGroup Upsert(FacetGroup facet);
    }
}