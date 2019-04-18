using System;
using System.Collections.Generic;
using System.Linq;
using Fame.Data.Models;
using Fame.Service.DTO;
using Fame.Service.Extensions;


namespace Fame.Service.Services
{
    public class ProductCombinationService : IProductCombinationService
    {
        //Extras will spawn a lot of combinations, so make sure it's the last thing we are handling, so we don't have to loop over extras over and over again
        private IDictionary<SelectionType, int> SELECTION_TYPE_PRIORITIZATION = new Dictionary<SelectionType, int>()
        {
            { SelectionType.RequiredOne, 0},
            { SelectionType.OptionalOne, 1},
            { SelectionType.OptionalMultiple, 10}
        };

        public IEnumerable<ProductCombination> GetCombinationsForComponentTypes(List<ComponentType> componentTypes,
            ProductVersion productVersion, Option[] startingPath, bool indexedOnly = false)
        {
            
            var componentTypeToSelectionType = productVersion
                .Groups
                .SelectMany(x => x.SectionGroups)
                .SelectMany(x => x.Sections)
                .ToDictionary(
                    x => x.ComponentType,
                    x => x.SelectionType
                );

            var componentTypeToOptions = productVersion.Options.ToLookup(o => o.Component.ComponentType);

            var parentComponentType = componentTypes.SingleOrDefault(x => x.ChildComponentTypes != null && x.ChildComponentTypes.Any());
            var nonParentComponentTypes = componentTypes
                .Where(x => x.ChildComponentTypes != null && !x.ChildComponentTypes.Any())
                .OrderBy(x => SELECTION_TYPE_PRIORITIZATION[componentTypeToSelectionType[x]])
                .ToArray();

            if (parentComponentType != null)
            {
                var parentOptions = componentTypeToOptions[parentComponentType];

                return parentOptions
                    .AsParallel()
                    .SelectMany(parentOption => GetPreviewsForOption(nonParentComponentTypes, componentTypeToOptions, componentTypeToSelectionType, parentOption, startingPath.Concat(new Option[] { parentOption }).ToArray(), indexedOnly))
                    .Select(combination => new ProductCombination()
                    {
                        Options = combination.ToList(),
                        ProductVersion = productVersion
                    });
            }
            else
            {
                return GetPreviewsForOption(nonParentComponentTypes, componentTypeToOptions, componentTypeToSelectionType, null, startingPath, indexedOnly)
                    .Select(combination => new ProductCombination()
                    {
                        Options = combination.ToList(),
                        ProductVersion = productVersion
                    });
            }

        }

        public IEnumerable<ProductCombination> GetCombinationsForOption(Option option, ProductVersion productVersion)
        {
            var relatedRenderSections = option.OptionRenderComponents.Select(x => x.ComponentType).ToList();
            return GetCombinationsForComponentTypes(relatedRenderSections, productVersion, new Option[] { option });
        }

        public IEnumerable<ProductCombination> GetCombinationsForProduct(ProductVersion productVersion, bool indexedonly = false)
        {
            var relatedRenderSections = productVersion.ProductRenderComponents.Select(x => x.ComponentType).ToList();
            return GetCombinationsForComponentTypes(relatedRenderSections, productVersion, new Option[] { }, indexedonly);
        }

        protected IEnumerable<Option[]> GetValidOptions(Option parentOption, IEnumerable<Option> previousPath, IEnumerable<Option> options, SelectionType selectionType, bool indexedOnly)
        {
            var validOptions = options
                .Where(o => o.Component.Indexed || !indexedOnly)
                .Where(o => IsValidCombination(parentOption, previousPath, o));

            if (selectionType == SelectionType.OptionalOne)
            {
                return validOptions
                    .Select(option => new Option[] { option })
                    .Append(Array.Empty<Option>());
            }
            else if (selectionType == SelectionType.RequiredOne)
            {
                return validOptions
                    .Select(option => new Option[] { option });
            }
            else if (selectionType == SelectionType.OptionalMultiple)
            {
                return validOptions
                    .PowerSet();
            }

            throw new ArgumentException("Unknown selection type for component");
        }

        protected IEnumerable<IEnumerable<Option>> GetPreviewsForOption(IEnumerable<ComponentType> renderSections, ILookup<ComponentType, Option> renderSectionToOptions, IDictionary<ComponentType, SelectionType> componentTypeToSelectType, Option parentOption, Option[] previousPath, bool indexedOnly)
        {
            var componentType = renderSections.FirstOrDefault();

            if (componentType != null)
            {
                var options = renderSectionToOptions[componentType];
                var selectionType = componentTypeToSelectType[componentType];
                var nextRenderSections = renderSections.Skip(1);
                var validOptions = GetValidOptions(parentOption, previousPath, options, selectionType, indexedOnly);

                return validOptions
                    .SelectMany(newOptions =>
                    {
                        var currentPath = new Option[previousPath.Length + newOptions.Length];
                        previousPath.CopyTo(currentPath, 0);
                        newOptions.CopyTo(currentPath, previousPath.Length);

                        return GetPreviewsForOption(nextRenderSections, renderSectionToOptions, componentTypeToSelectType, parentOption, currentPath, indexedOnly);
                    });
            }
            else
            {
                return new IEnumerable<Option>[] { previousPath };
            }
        }

        protected bool IsValidCombination(Option parentOption, IEnumerable<Option> options, Option newOption)
        {
            if (parentOption != null && !newOption.CompatibleOptions.Any(x => x.ParentOptionId == parentOption.OptionId || x.ParentOptionId == null))
            {
                return false;
            }

            var incompatibilities = options
                .Append(newOption)
                .SelectMany(o => o.Incompatibilities)
                .Where(x => parentOption == null || x.ParentOptionId == parentOption.OptionId || x.ParentOptionId == null);

            foreach (var incompatibility in incompatibilities)
            {
                if (CheckIncompatibility(incompatibility, options, newOption))
                {
                    return false;
                }
            }


            //TODO this doesn't work with incompatibilities within newOptions

            return true;
        }

        protected bool CheckIncompatibility(Incompatibility incompatiblity, IEnumerable<Option> options, Option newOption)
        {
            return (incompatiblity.Option == newOption || options.Contains(incompatiblity.Option))
                     && (incompatiblity.IncompatibleOptions.All(io => newOption == io.Option || options.Contains(io.Option)));
        }
    }
}
