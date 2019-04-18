using Fame.Data.Models;

namespace Fame.Service.Services
{
    public interface IFacetCategoryService : IBaseService<FacetCategory>
    {
        FacetCategory Upsert(FacetCategory facetCategory);
    }
}