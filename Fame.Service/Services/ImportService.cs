using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Fame.Common;
using Fame.Data.Models;
using Fame.Service.ChangeTracking;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fame.Service.Services
{
    public partial class ImportService : IImportService
    {
        private readonly IOccasionService _occasionService;
        private readonly ICollectionService _collectionService;
        private readonly ICollectionFacetService _collectionFacetService;
        private readonly ICollectionFacetBoostService _collectionFacetBoostService;
        private readonly ICollectionOccasionService _collectionOccasionService;
        private readonly ICollectionProductService _collectionProductService;
        private readonly IProductService _productService;
        private readonly IComponentTypeService _componentTypeService;
        private readonly IComponentMetaService _componentMetaService;
        private readonly IFacetBoostService _facetBoostService;
        private readonly IFacetMetaService _facetMetaService;
        private readonly IComponentService _componentService;
        private readonly IProductVersionService _productVersionService;
        private readonly IProductRenderComponentService _productRenderComponentService;
        private readonly IRenderPositionService _renderPositionService;
        private readonly ISectionService _sectionService;
        private readonly IOptionService _optionService;
        private readonly IIncompatibilityService _incompatibilityService;
        private readonly ICompatibleOptionService _compatibleOptionService;
        private readonly IFacetService _facetService;
        private readonly IFacetGroupService _facetGroupService;
        private readonly IFacetCategoryService _facetCategoryService;
        private readonly IFacetConfigurationService _facetConfigurationService;
        private readonly IFacetCategoryGroupService _facetCategoryGroupService;
        private readonly IFacetCategoryConfigurationService _facetCategoryConfigurationService;
        private readonly IManufacturingSortOrderService _manufacturingSortOrderService;
        private readonly IProductVersionPriceService _productVersionPriceService;
        private readonly FameConfig _fameConfig;
        private readonly ICacheService _cacheService;
        private readonly ILogger<ImportService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurationService _curationService;
        private readonly IWorkflowService _workflowService;

        public ImportService(
            ICacheService cacheService,
            IBaseServices services,
            IOptions<FameConfig> fameConfig,
            ILogger<ImportService> logger,
            IUnitOfWork unitOfWork)
        {
            _collectionOccasionService = services.CollectionOccasion.Value;
            _occasionService = services.Occasion.Value;
            _collectionService = services.Collection.Value;
            _collectionProductService = services.CollectionProduct.Value;
            _collectionFacetService = services.CollectionFacet.Value;
            _collectionFacetBoostService = services.CollectionFacetBoost.Value;
            _productService = services.Product.Value;
            _facetMetaService = services.FacetMeta.Value;
            _facetBoostService = services.FacetBoost.Value;
            _componentTypeService = services.ComponentType.Value;
            _componentService = services.Component.Value;
            _productVersionService = services.ProductVersion.Value;
            _productRenderComponentService = services.ProductRenderComponent.Value;
            _renderPositionService = services.RenderPosition.Value;
            _sectionService = services.Section.Value;
            _optionService = services.Option.Value;
            _incompatibilityService = services.Incompatibility.Value;
            _compatibleOptionService = services.CompatibleOption.Value;
            _facetService = services.Facet.Value;
            _facetGroupService = services.FacetGroup.Value;
            _facetCategoryService = services.FacetCategory.Value;
            _facetConfigurationService = services.FacetConfiguration.Value;
            _componentMetaService = services.ComponentMeta.Value;
            _facetCategoryGroupService = services.FacetCategoryGroup.Value;
            _facetCategoryConfigurationService = services.FacetCategoryConfiguration.Value;
            _manufacturingSortOrderService = services.ManufacturingSortOrder.Value;
            _productVersionPriceService = services.ProductVersionPrice.Value;
            _fameConfig = fameConfig.Value;
            this._cacheService = cacheService;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _curationService = services.Curation.Value;
            _workflowService = services.Workflow.Value;
        }

        public void Import()
        {
            _logger.LogInformation("Importing...");
            _unitOfWork.BeginTransaction();
            var collectionTask = GetData<CollectionImport>();
            var occasionTask = GetData<OccasionImport>();
            var facetBoostTask = GetData<FacetBoostImport>();
            var facetCategoryTask = GetData<FacetCategoryImport>();
            var facetConfigurationTask = GetData<FacetConfigurationImport>();
            var facetGroupsTask = GetData<FacetGroupImport>();
            var facetsTask = GetData<FacetImport>();
            var productsTask = GetData<ProductImport>();
            var componentsTask = GetData<ComponentImport>();
            var componentTypesTask = GetData<ComponentTypeImport>();
            var compatibleComponentsTask = GetData<CompatibleComponentImport>();
            var incompatibilitiesTask = GetData<IncompatibilityImport>();
            var groupsTask = GetData<GroupImport>();
            var manufacturingSortOrdersTask = GetData<ManufacturingSortOrderImport>();
            var sectionGroupsTask = GetData<SectionGroupImport>();
            var relatedRenderComponentGroupTypeTask = GetData<RelatedRenderComponentGroupsImport>();

            Task.WaitAll(
                collectionTask,
                occasionTask,
                facetBoostTask,
                facetsTask,
                facetCategoryTask,
                facetConfigurationTask,
                facetGroupsTask,
                productsTask,
                componentsTask,
                componentTypesTask,
                compatibleComponentsTask,
                incompatibilitiesTask,
                groupsTask,
                manufacturingSortOrdersTask,
                sectionGroupsTask,
                relatedRenderComponentGroupTypeTask
            );

            var collections = collectionTask.Result;
            var occasions = occasionTask.Result;
            var facetBoosts = facetBoostTask.Result;
            var facetCategories = facetCategoryTask.Result;
            var facetConfigurations = facetConfigurationTask.Result;
            var facets = facetsTask.Result;
            var facetGroups = facetGroupsTask.Result;
            var products = productsTask.Result;
            var components = componentsTask.Result;
            var componentTypes = componentTypesTask.Result;
            var compatibleComponents = compatibleComponentsTask.Result;
            var incompatibilities = incompatibilitiesTask.Result;
            var groups = groupsTask.Result;
            var manufacturingSortOrders = manufacturingSortOrdersTask.Result;
            var sectionGroups = sectionGroupsTask.Result;
            var relatedRenderComponentGroupTypes = relatedRenderComponentGroupTypeTask.Result;

            var renderPositions = new List<RenderPosition>
                {
                    new RenderPosition { RenderPositionId = "SwatchNone", Orientation = Orientation.Swatch, Zoom = Zoom.None },
                    new RenderPosition { RenderPositionId = "CadNone", Orientation = Orientation.Cad, Zoom = Zoom.None },
                    new RenderPosition { RenderPositionId = "FrontNone", Orientation = Orientation.Front, Zoom = Zoom.None },
                    new RenderPosition { RenderPositionId = "FrontBottom", Orientation = Orientation.Front, Zoom = Zoom.Bottom },
                    new RenderPosition { RenderPositionId = "FrontMiddle", Orientation = Orientation.Front, Zoom = Zoom.Middle },
                    new RenderPosition { RenderPositionId = "FrontTop", Orientation = Orientation.Front, Zoom = Zoom.Top },
                    new RenderPosition { RenderPositionId = "BackNone", Orientation = Orientation.Back, Zoom = Zoom.None },
                    new RenderPosition { RenderPositionId = "BackBottom", Orientation = Orientation.Back, Zoom = Zoom.Bottom },
                    new RenderPosition { RenderPositionId = "BackMiddle", Orientation = Orientation.Back, Zoom = Zoom.Middle },
                    new RenderPosition { RenderPositionId = "BackTop", Orientation = Orientation.Back, Zoom = Zoom.Top },
                };

            // Delete all Collections, Occasions, FacetGroup, FacetCategory and FacetConfiguration
            _collectionService.DeleteAll();
            _collectionProductService.DeleteAll();
            _collectionFacetService.DeleteAll();
            _collectionOccasionService.DeleteAll();
            _occasionService.DeleteAll();
            _facetCategoryGroupService.DeleteAll();
            _facetCategoryConfigurationService.DeleteAll();
            _collectionFacetBoostService.DeleteAll();
            _facetBoostService.DeleteAll();
            _unitOfWork.Save();

            collections.ForEach(c => _collectionService.Insert(new Collection { CollectionId = c.CollectionId, CollectionName = c.CollectionName }));
            occasions.ForEach(c => _occasionService.Insert(new Occasion
            {
                OccasionId = c.OccasionId,
                OccasionName = c.OccasionName,
                ComponentCompatibilityRule = c.ComponentCompatibilityRule,
                FacetCompatibilityRule = c.FacetCompatibilityRule,
                Collections = collections.Where(cl => c.CollectionIds.Contains(cl.CollectionId)).Select(cl => new CollectionOccasion { OccasionId = c.OccasionId, CollectionId = cl.CollectionId }).ToList()
            }));

            // Related render types is just a lookup used for reducing duplication in the spreadsheet and doesn't have a co-related table in the DB so just turn into a dictionary.
            var relatedRenderComponentTypesLookup = relatedRenderComponentGroupTypes.ToDictionary(c => c.RenderTypeId, c => c);

            // Upsert Render Positions
            var renderPositionLookups = renderPositions.ToDictionary(c => c.RenderPositionId, c => _renderPositionService.Upsert(c));

            // Upsert manufacturing Sort Orders
            var manufacturingSortOrderLookups = manufacturingSortOrders.ToDictionary(c => c.Id, c => _manufacturingSortOrderService.Upsert(new ManufacturingSortOrder { Order = c.Order, Id = c.Id }));

            var groupLookups = groups.ToDictionary(c => c.GroupName, c => c);

            // Upsert new Component ComponentTypeId
            var componentTypesLookup = componentTypes.ToDictionary(
                c => c.ComponentTypeId,
                c => _componentTypeService.Upsert(new ComponentType
                {
                    ComponentTypeId = c.ComponentTypeId,
                    ComponentTypeCategory = c.ComponentTypeCategory,
                    Title = c.Title,
                    SelectionTitle = c.SelectionTitle,
                    IsProductCode = c.IsProductCode,
                    ParentComponentTypeId = c.ParentComponentTypeId,
                    SortWeightDefault = c.SortWeigthDefault,
                    SortWeightOther = c.SortWeigthOther,
                    AggregateOnIndex = c.AggregateOnIndex
                }));

            _unitOfWork.Save();

            //Insert FacetBoosts
            facetBoosts.ForEach(fb => _facetBoostService.Insert(new FacetBoost
            {
                BoostRule = fb.BoostRule,
                BoostWeight = fb.BoostWeight,
                Collections = fb.CollectionIds.Select(c => new CollectionFacetBoost { CollectionId = c }).ToList()
            }));

            // Upsert FacetGroups
            var facetGroupsLookups = facetGroups.ToDictionary(c => c.FacetGroupId, c => _facetGroupService.Upsert(new FacetGroup
            {
                FacetGroupId = c.FacetGroupId,
                Title = c.Title,
                Sort = c.Sort,
                Subtitle = c.Subtitle,
                IsAggregatedFacet = c.IsAggregatedFacet,
                IsCategoryFacet = c.IsCategoryFacet,
                Collapsed = c.Collapsed,
                Multiselect = c.Multiselect,
                Slug = c.Slug,
                Name = c.Name,
                ProductNameOrder = c.ProductNameOrder,
                PrimarySilhouette = c.PrimarySilhouette
            }));

            // Upsert FacetCategories
            var facetCategoryLookups = facetCategories
                .ToDictionary(
                    c => c.FacetCategoryId,
                    c => _facetCategoryService
                        .Upsert(new FacetCategory
                        {
                            FacetCategoryGroups = c.FacetGroupIds.Select(g => new FacetCategoryGroup { FacetGroupId = g, FacetCategoryId = c.FacetCategoryId }).ToList(),
                            FacetCategoryId = c.FacetCategoryId,
                            Sort = c.Sort,
                            Title = c.Title,
                            HideHeader = c.HideHeader
                        }));

            // Upsert FacetConfigurations
            var facetConfigurationLookups = facetConfigurations
                .ToDictionary(
                    c => c.FacetConfigurationId,
                    c => _facetConfigurationService
                        .Upsert(new FacetConfiguration
                        {
                            FacetCategoryConfigurations = c.FacetCategoryIds.Select(g => new FacetCategoryConfiguration { FacetCategoryId = g, FacetConfigurationId = c.FacetConfigurationId }).ToList(),
                            Title = c.Title,
                            FacetConfigurationId = c.FacetConfigurationId,
                        }));

            // Upsert Facets
            _facetMetaService.DeleteAll();
            _curationService.RemovePrimarySilhouetteIds();
            _facetService.DeleteAll();
            _unitOfWork.Save();

            var facetLookups = facets.ToDictionary(c => c.FacetId, c => _facetService.Upsert(new Facet
            {
                FacetId = c.FacetId,
                Title = c.Title,
                Subtitle = c.Subtitle,
                FacetGroup = facetGroupsLookups[c.FacetGroupId],
                Order = c.Order,
                PreviewImage = c.PreviewImage,
                CompatibilityRule = c.CompatibilityRule,
                FacetMeta = c.FacetMeta.Select(m => new FacetMeta { FacetId = c.FacetId, Key = m.Key, Value = m.Value }).ToList(),
                TagPriority = c.TagPriority,
                Description = c.Description,
                Name = c.Name,
                TaxonString = c.TaxonString,
                Collections = collections.Where(cl => c.CollectionIds.Contains(cl.CollectionId)).Select(cl => new CollectionFacet { CollectionId = cl.CollectionId, FacetId = c.FacetId }).ToList()
            }));

            // Upsert Products

            products.ForEach(c => _productService.Upsert(new Product { ProductId = c.ProductId, Title = c.Title, Index = c.Index, ProductType = c.ProductType, PreviewType = c.PreviewType, DropBoxAssetFolder = c.DropBoxAssetFolder, DropName = c.DropName, DisableLayering = c.DisableLayering, Collections = collections.Where(cl => c.CollectionIds.Contains(cl.CollectionId)).Select(cl => new CollectionProduct { CollectionId = cl.CollectionId, ProductId = c.ProductId }).ToList() }));

            // Create new Product Version for each Product
            var productVersionLookups = products.ToDictionary(c => c.ProductId, c => _productVersionService.CreateNewVersion(c.ProductId, c.Factory, c.Active));

            _unitOfWork.Save(); // Call save in order to generate product version Id's

            foreach (var productImport in products)
            {
                _productVersionPriceService.Insert(new ProductVersionPrice { LocalisationCode = _fameConfig.Localisation.AU, ProductVersion = productVersionLookups[productImport.ProductId], PriceInMinorUnits = productImport.AUD });
                _productVersionPriceService.Insert(new ProductVersionPrice { LocalisationCode = _fameConfig.Localisation.US, ProductVersion = productVersionLookups[productImport.ProductId], PriceInMinorUnits = productImport.USD });
            }

            // Add Product Render Components
            foreach (var p in products)
            {
                var relatedRenderGroup = relatedRenderComponentTypesLookup[p.RelatedRenderComponentGroupType];
                foreach (var productRenderComponent in relatedRenderGroup.RelatedRenderComponents)
                {
                    var prc = new ProductRenderComponent
                    {
                        ComponentTypeId = productRenderComponent,
                        ProductVersionId = productVersionLookups[p.ProductId].ProductVersionId
                    };
                    _productRenderComponentService.Insert(prc);
                }
            }

            // Delete all Meta before adding new components
            _componentMetaService.DeleteAll();
            _unitOfWork.Save();

            // Upsert Components
            var componentLookups = components.ToDictionary(c => c.ComponentId, c => _componentService.Upsert(
                    new Component
                    {
                        ComponentId = c.ComponentId,
                        Title = c.Title,
                        ComponentTypeId = c.ComponentTypeId,
                        RenderPosition = renderPositionLookups[relatedRenderComponentTypesLookup[c.OptionRenderComponentType].RenderPosition],
                        CartId = c.CartId,
                        Sort = c.SortOrder,
                        Indexed = c.Indexed,
                        ComponentMeta = c.ComponentMeta.Select(m => new ComponentMeta { ComponentId = c.ComponentId, Key = m.Key, Value = m.Value }).ToList(),
                        ManufacturingSortOrder = manufacturingSortOrderLookups[c.ManufacturingSortOrderId],
                        IsolateInSummary = c.IsolateInSummary,
                        PreviewZoom = c.PreviewZoom
                    }));

            _unitOfWork.Save();

            var productVersionOptionLookup = new Dictionary<string, Option>();
            // Insert Options
            foreach (var compatibleComponent in compatibleComponents)
            {
                foreach (var componentId in compatibleComponent.ComponentIds)
                {
                    var productVersionOptionKey = $"{compatibleComponent.ProductId}-{componentId}";
                    if (productVersionOptionLookup.ContainsKey(productVersionOptionKey)) continue;

                    var component = componentLookups[componentId];
                    var componentImport = components.Single(c => c.ComponentId == componentId);

                    var optionRenderComponentId = componentImport.OptionRenderComponentType;
                    var optionRenderComponents = relatedRenderComponentTypesLookup[optionRenderComponentId]
                        .RelatedRenderComponents
                        .Select(c => new OptionRenderComponent { ComponentTypeId = c })
                        .ToList();

                    var option = new Option
                    {
                        Component = component,
                        Title = component.Title,
                        ProductVersion = productVersionLookups[compatibleComponent.ProductId],
                        OptionRenderComponents = optionRenderComponents,
                        Prices = new List<OptionPrice>
                            {
                                new OptionPrice { LocalisationCode = _fameConfig.Localisation.AU, PriceInMinorUnits = componentImport.AUD },
                                new OptionPrice { LocalisationCode = _fameConfig.Localisation.US, PriceInMinorUnits = componentImport.USD }
                            }
                    };

                    _optionService.Insert(option);
                    productVersionOptionLookup.Add(productVersionOptionKey, option);
                }
            }

            _unitOfWork.Save();

            foreach (var i in incompatibilities)
            {
                foreach (var incompatibleComponentIds in i.IncompatibleWith.Where(iw => iw.Any() && iw.All(inc => !string.IsNullOrWhiteSpace(inc))))
                {
                    var parentOption = string.IsNullOrEmpty(i.ParentComponentId) ? null : productVersionOptionLookup[$"{i.ProductId}-{i.ParentComponentId}"];
                    _incompatibilityService.Insert(new Incompatibility
                    {
                        Option = productVersionOptionLookup[$"{i.ProductId}-{i.ComponentId}"],
                        IncompatibleOptions = incompatibleComponentIds.Select(ic => new IncompatibleOption { Option = productVersionOptionLookup[$"{i.ProductId}-{ic}"] }).ToList(),
                        ParentOption = parentOption
                    });
                }
            }

            var productVersionSectionLookup = new Dictionary<string, Section>();
            // Upsert Groups, SectionGroups & Sections
            foreach (var product in products)
            {
                foreach (var groupName in product.Groups)
                {
                    // Group
                    var groupImport = groupLookups[groupName];
                    var group = new Group
                    {
                        Order = groupImport.Order,
                        Title = groupImport.Title,
                        ProductVersion = productVersionLookups[product.ProductId],
                        Slug = groupImport.Slug,
                        SelectionTitle = groupImport.SelectionTitle,
                        Hidden = groupImport.Hidden
                    };
                    foreach (var sectionGroupImport in sectionGroups.Where(sg => sg.GroupName == groupName))
                    {
                        // SectionGroup
                        var sectionGroup = new SectionGroup
                        {
                            Group = group,
                            Title = sectionGroupImport.Title,
                            Order = sectionGroupImport.Order,
                            RenderPosition = renderPositionLookups[sectionGroupImport.RenderPosition],
                            Slug = sectionGroupImport.Slug,
                            AggregateTitle = sectionGroupImport.AggregateTitle
                        };
                        var sectionOrder = 0;
                        foreach (var sectionComponentTypeId in sectionGroupImport.SectionComponentTypeIds)
                        {
                            // Section
                            var componentTypeImport = componentTypes.Single(ct => ct.ComponentTypeId == sectionComponentTypeId);
                            sectionOrder += 1;
                            var sectionComponentType = componentTypesLookup[sectionComponentTypeId];
                            var section = new Section
                            {
                                ComponentType = sectionComponentType,
                                Order = sectionOrder,
                                Title = componentTypeImport.SelectionTitle,
                                SectionGroup = sectionGroup,
                                SelectionType = componentTypeImport.SelectionType
                            };
                            _sectionService.Insert(section);
                            productVersionSectionLookup.Add($"{product.ProductId}-{sectionComponentTypeId}", section);
                        }
                    }
                }
            }

            _unitOfWork.Save();

            // Insert compatible components for sections
            foreach (var compatibleComponentImport in compatibleComponents)
            {
                foreach (var compatibleComponentId in compatibleComponentImport.ComponentIds)
                {
                    var parentComponentId = compatibleComponentImport.ParentComponentId;
                    var hasParentComponent = !string.IsNullOrEmpty(parentComponentId);
                    var productId = compatibleComponentImport.ProductId;
                    var option = productVersionOptionLookup[$"{productId}-{compatibleComponentId}"];
                    var section = productVersionSectionLookup[$"{productId}-{option.Component.ComponentTypeId}"];
                    var isDefault = components.Single(c => c.ComponentId == compatibleComponentId).DefaultOptionIn.Any(defaultParentComponentId => defaultParentComponentId == parentComponentId);
                    var parentOption = hasParentComponent ? productVersionOptionLookup[$"{productId}-{parentComponentId}"] : null;
                    _compatibleOptionService.Insert(new CompatibleOption
                    {
                        IsDefault = isDefault,
                        Option = option,
                        ParentOption = parentOption,
                        Section = section
                    });
                }
            }
            _unitOfWork.Save();
            _unitOfWork.CommitTransaction();

            _cacheService.DeleteAll();
        }

        private async Task<List<T>> GetData<T>() where T : IDataImport<T>, new()
        {
            var tabTitle = new T().TabTitle;
            var url = _fameConfig.Import.Url + tabTitle;
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(url))
            using (var content = response.Content)
            {
                var data = (await content.ReadAsStringAsync()).Replace("\"", "");
                var dataRows = data.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList().Skip(1);
                return dataRows.Select(d => new T().FromCsv(d)).ToList();
            }
        }
    }
}
