using System;
using System.Linq;
using System.Threading.Tasks;
using Fame.Common;
using Fame.Data.Models;
using Fame.Data.Repository;
using Fame.Service.DTO;
using Fame.Service.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Fame.Service.Services
{
    public class SpreeExportService : ISpreeExportService
    {
        private readonly IProductVersionRepository _repo;
        private readonly FameConfig _fameConfig;

        public SpreeExportService(IRepositories repositories, IOptions<FameConfig> fameConfig)
        {
            _repo = repositories.ProductVersion.Value;
            _fameConfig = fameConfig.Value;
        }

        async public Task<SpreeImport> GetExport(int productVersionId)
        {
            var productVersion = await _repo.Get()
                .Include(x => x.Prices)
                .Include(x => x.Options)
                    .ThenInclude(x => x.Component)
                        .ThenInclude(x => x.ComponentType)
                .Include(x => x.Options)
                    .ThenInclude(x => x.Component)
                        .ThenInclude(x => x.ManufacturingSortOrder)
                .Include(x => x.Options)
                    .ThenInclude(x => x.Component)
                        .ThenInclude(x => x.ComponentMeta)
                .Include(x => x.Options)
                    .ThenInclude(x => x.Prices)
                .Include(x => x.Product)
                    .ThenInclude(x => x.Curations)
                        .ThenInclude(x => x.Media)
                            .ThenInclude(x => x.CurationMediaVariants)
                .Where(x => x.ProductVersionId == productVersionId)

                .FirstAsync();

            var fabrics = productVersion.Options.Where(x => x.Component.ComponentType.ComponentTypeCategory == ComponentTypeCategory.Fabric);
            var colors = productVersion.Options.Where(x => x.Component.ComponentType.ComponentTypeCategory == ComponentTypeCategory.Color);

            var customizations = productVersion.Options.Where(x => x.Component.ComponentType.ComponentTypeCategory == ComponentTypeCategory.Customization || x.Component.ComponentType.ComponentTypeCategory == ComponentTypeCategory.Length);
            var makingOptions = productVersion.Options.Where(x => x.Component.ComponentType.ComponentTypeCategory == ComponentTypeCategory.Making);

            if (productVersion.Prices == null) throw new ApplicationException($"Product Version for {productVersion.ProductId} must have a price");

            try
            {
                var spreeImport = new SpreeImport()
                {
                    Details = new SpreeImport.ProductDetails()
                    {
                        Fabrics = fabrics.Select(MapOption),
                        Colors = colors.Select(MapOption),
                        Fabric = "",
                        Factory = productVersion.Factory,
                        Fit = "",
                        ShortDescription = "",
                        StyleNotes = "",
                        Active = productVersion.VersionState == VersionState.Active,
                        Name = productVersion.Product.Title,
                        PrimaryImage = null,
                        SecondaryImages = Array.Empty<String>(),
                        Taxons = Array.Empty<String>(),
                        Type = productVersion.Product.ProductType,
                        PriceAud = FormatPrice(productVersion.Prices.InAUD()),
                        PriceUsd = FormatPrice(productVersion.Prices.InUSD()),
                    },
                    CustomizationList = customizations.Select(MapOption),
                    MakingOptions = makingOptions.Select(MapOption),
                    StyleNumber = productVersion.Product.ProductId,
                    Curations = productVersion.Product.Curations.Select(MapCuration)
                };

                return spreeImport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SpreeImport.Curation MapCuration(Curation curation)
        {
            return new SpreeImport.Curation()
            {
                Pid = curation.PID,
                Name = curation.Name,
                Description = curation.Description,
                Taxons = curation.Taxons,
                Media = curation.Media.OrderBy(m => m.PLPSortOrder).Select(m => m.CurationMediaVariants.First(cmv => cmv.IsOriginal)?.ToFullPath(_fameConfig.Curations.Url)).Where(x => x != null).Take(2)
            };
        }

        private SpreeImport.Customization MapOption(Option option)
        {
            return new SpreeImport.Customization()
            {
                Code = option.Component.CartId ?? option.ComponentId,
                CustomizationId = option.OptionId.ToString(),
                CustomizationPresentation = option.Title,
                ManifacturingSortOrder = option.Component.ManufacturingSortOrder.Order,
                PriceAud = FormatPrice(option.Prices.InAUD()),
                PriceUsd = FormatPrice(option.Prices.InUSD()),
                Hex = option.Component.ComponentMeta.FirstOrDefault(x => x.Key == "hex")?.Value
            };
        }
        private string FormatPrice(OptionPrice optionPrice)
        {
            return (optionPrice.PriceInMinorUnits / 100).ToString("#.##");
        }
        private string FormatPrice(ProductVersionPrice optionPrice)
        {
            return (optionPrice.PriceInMinorUnits / 100).ToString("#.##");
        }
    }
}
