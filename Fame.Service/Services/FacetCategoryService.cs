using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class FacetCategoryService : BaseService<FacetCategory>, IFacetCategoryService
    {
        private readonly IRepositories _repositories;
        private readonly IFacetCategoryRepository _facetCategoryRepo;

        public FacetCategoryService(IRepositories repositories)
            : base(repositories.FacetCategory.Value)
        {
            _repositories = repositories;
            _facetCategoryRepo = _repositories.FacetCategory.Value;
        }

        public FacetCategory Upsert(FacetCategory facetCategory)
        {
            var dbFacetCategory = _facetCategoryRepo.FindById(facetCategory.FacetCategoryId);

            // Insert if exists
            if (dbFacetCategory == null)
            {
                _facetCategoryRepo.Insert(facetCategory);
                return facetCategory;
            }

            // Otherwise Update
            dbFacetCategory.HideHeader = facetCategory.HideHeader;
            dbFacetCategory.Title = facetCategory.Title;
            dbFacetCategory.Sort = facetCategory.Sort;
            dbFacetCategory.FacetCategoryGroups = facetCategory.FacetCategoryGroups;
            _facetCategoryRepo.Update(dbFacetCategory);
            return dbFacetCategory;
        }
    }
}