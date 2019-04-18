using System;

namespace Fame.Service.Services
{
    public interface IBaseServices
    {
        Lazy<IComponentService> Component { get; set; }
        Lazy<IComponentTypeService> ComponentType { get; set; }
        Lazy<IGroupService> Group { get; set; }
        Lazy<IIncompatibilityService> Incompatibility { get; set; }
        Lazy<IOptionService> Option { get; set; }
        Lazy<IOptionPriceService> OptionPrice { get; set; }
        Lazy<IProductService> Product { get; set; }
        Lazy<IProductVersionService> ProductVersion { get; set; }
        Lazy<IProductRenderComponentService> RelatedRenderComponent { get; set; }
        Lazy<ISectionService> Section { get; set; }
        Lazy<ISectionGroupService> SectionGroup { get; set; }
        Lazy<ICompatibleOptionService> CompatibleOption { get; set; }
        Lazy<IProductSummaryService> ProductSummary { get; set; }
        Lazy<IImportService> Import { get; set; }
        Lazy<IRenderPositionService> RenderPosition { get; set; }
        Lazy<IFacetService> Facet { get; set; }
        Lazy<IFacetGroupService> FacetGroup { get; set; }
        Lazy<IComponentMetaService> ComponentMeta { get; set; }
        Lazy<IProductRenderComponentService> ProductRenderComponent { get; set; }
        Lazy<IFacetConfigurationService> FacetConfiguration { get; set; }
        Lazy<IFacetCategoryService> FacetCategory { get; set; }
        Lazy<IFacetCategoryConfigurationService> FacetCategoryConfiguration { get; set; }
        Lazy<IFacetCategoryGroupService> FacetCategoryGroup { get; set; }
        Lazy<IManufacturingSortOrderService> ManufacturingSortOrder { get; set; }
        Lazy<IProductVersionPriceService> ProductVersionPrice { get; set; }
        Lazy<ICacheService> Cache { get; set; }
        Lazy<IFacetMetaService> FacetMeta { get; set; }
        Lazy<IFacetBoostService> FacetBoost { get; set; }
        Lazy<ICurationMediaService> CurationMedia { get; set; }
        Lazy<ICurationService> Curation { get; set; }
        Lazy<IOccasionService> Occasion { get; set; }
        Lazy<ICollectionService> Collection { get; set; }
        Lazy<ICollectionFacetService> CollectionFacet { get; set; }
        Lazy<ICollectionFacetBoostService> CollectionFacetBoost { get; set; }
        Lazy<ICollectionProductService> CollectionProduct { get; set; }
        Lazy<ICollectionOccasionService> CollectionOccasion { get; set; }
        Lazy<IFeedMetaService> FeedMeta { get; set; }
        Lazy<IWorkflowService> Workflow { get; set; }
    }
}
