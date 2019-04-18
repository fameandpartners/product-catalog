using System;

namespace Fame.Data.Repository
{
    public interface IRepositories
    {
        Lazy<IComponentRepository> Component { get; set; }
        Lazy<IComponentTypeRepository> ComponentType { get; set; }
        Lazy<IGroupRepository> Group { get; set; }
        Lazy<IIncompatibilityRepository> Incompatibility { get; set; }
        Lazy<IOptionRepository> Option { get; set; }
        Lazy<IOptionPriceRepository> OptionPrice { get; set; }
        Lazy<IProductRepository> Product { get; set; }
        Lazy<IProductVersionRepository> ProductVersion { get; set; }
        Lazy<IProductRenderComponentRepository> ProductRenderComponent { get; set; }
        Lazy<IOptionRenderComponentRepository> OptionRenderComponent { get; set; }
        Lazy<ISectionRepository> Section { get; set; }
        Lazy<ISectionGroupRepository> SectionGroup { get; set; }
        Lazy<ICompatibleOptionRepository> CompatibleOption { get; set; }
        Lazy<IRenderPositionRepository> RenderPosition { get; set; }
        Lazy<IFacetRepository> Facet { get; set; }
        Lazy<IFacetGroupRepository> FacetGroup { get; set; }
        Lazy<IComponentMetaRepository> ComponentMeta { get; set; }
        Lazy<IFacetConfigurationRepository> FacetConfiguration { get; set; }
        Lazy<IFacetCategoryRepository> FacetCategory { get; set; }
        Lazy<IFacetCategoryConfigurationRepository> FacetCategoryConfiguration { get; set; }
        Lazy<IFacetCategoryGroupRepository> FacetCategoryGroup { get; set; }
        Lazy<IManufacturingSortOrderRepository> ManufacturingSortOrder { get; set; }
        Lazy<IProductVersionPriceRepository> ProductVersionPrice { get; set; }
        Lazy<IFacetMetaRepository> FacetMeta { get; set; }
        Lazy<IFacetBoostRepository> FacetBoost { get; set; }
        Lazy<ICurationRepository> Curation { get; set; }
        Lazy<ICurationMediaRepository> CurationMedia { get; set; }
        Lazy<ICurationMediaVariantRepository> CurationMediaVariant { get; set; }
        Lazy<ICurationComponentRepository> CurationComponent { get; set; }
        Lazy<IOccasionRepository> Occasion { get; set; }
        Lazy<ICollectionRepository> Collection { get; set; }
        Lazy<ICollectionFacetRepository> CollectionFacet { get; set; }
        Lazy<ICollectionFacetBoostRepository> CollectionFacetBoost { get; set; }
        Lazy<ICollectionProductRepository> CollectionProduct { get; set; }
        Lazy<ICollectionOccasionRepository> CollectionOccasion { get; set; }
        Lazy<IFeedMetaRepository> FeedMeta { get; set; }
        Lazy<IWorkflowRepository> Workflow { get; set; }
    }
}
