using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class FacetGroupService : BaseService<FacetGroup>, IFacetGroupService
    {
        private readonly IFacetGroupRepository _facetGroupRepo;

        public FacetGroupService(IRepositories repositories)
            : base(repositories.FacetGroup.Value)
        {
            _facetGroupRepo = repositories.FacetGroup.Value;
        }

        public FacetGroup Upsert(FacetGroup facetGroup)
        {
            var dbFacetGroup = _facetGroupRepo.FindById(facetGroup.FacetGroupId);

            // Insert if exists
            if (dbFacetGroup == null)
            {
                _facetGroupRepo.Insert(facetGroup);
                return facetGroup;
            }

            // Otherwise Update
            dbFacetGroup.Title = facetGroup.Title;
            dbFacetGroup.Subtitle = facetGroup.Subtitle;
            dbFacetGroup.Sort = facetGroup.Sort;
            dbFacetGroup.IsAggregatedFacet = facetGroup.IsAggregatedFacet;
            dbFacetGroup.IsCategoryFacet = facetGroup.IsCategoryFacet;
            dbFacetGroup.Multiselect = facetGroup.Multiselect;
            dbFacetGroup.Collapsed = facetGroup.Collapsed;
            dbFacetGroup.Slug = facetGroup.Slug;
            dbFacetGroup.Name = facetGroup.Name;
            dbFacetGroup.ProductNameOrder = facetGroup.ProductNameOrder;
            dbFacetGroup.PrimarySilhouette = facetGroup.PrimarySilhouette;
            _facetGroupRepo.Update(dbFacetGroup);
            return dbFacetGroup;
        }
    }
}
