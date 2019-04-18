using System;
using System.Collections.Generic;
using System.Linq;
using Fame.Data.Models;

namespace Fame.Service.DTO
{
    [Serializable]
    public class PIDModel : ISearchMeta
    {
        public string PID { get; }

        private int Price { get; set; }

        public PIDModel(string pid, IReadOnlyDictionary<string, ProductVersion> productVersions, string locale = null)
        {
            var pidItems = pid.Split("~");
            ValidateCombination(pidItems, productVersions);
            if (InvalidCombination) return;
            ProductId = pidItems.First();
            ComponentIds = pidItems.Skip(1).ToList();
            ProductVersionId = productVersions[ProductId].ProductVersionId;
            PreviewType = productVersions[ProductId].Product.PreviewType;
            PID = pid;
            var indexedComponents = productVersions[ProductId]
                .Options.Where(o => ComponentIds.Contains(o.ComponentId))
                .Where(o => o.Component.Indexed)
                .Select(o => o.ComponentId)
                .ToList();
            ProductDocumentVersionId = $"{ProductId}~{string.Join("~", indexedComponents.OrderBy(c => c))}";
            if (!string.IsNullOrEmpty(locale)) ProductPrice = productVersions[ProductId].Prices.Single(p => p.LocalisationCode == locale).PriceInMinorUnits;
        }

        public string ProductDocumentVersionId { get; set; }

        public PreviewType PreviewType { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        private int ProductPrice { get; }

        private string ProductId { get; }

        public int ProductVersionId { get; }

        public string PrimarySilhouetteId { get; set; }

        public List<string> ComponentIds { get; }

        public string TaxonString { get; set; }

        public void SetPrice(IReadOnlyDictionary<string, int> prices)
        {
            int price = 0;
            if (InvalidCombination) return;
            foreach (var componentId in ComponentIds)
            {
                if (prices.ContainsKey(componentId))
                {
                    price = price + prices[componentId];
                }
                else
                {
                    InvalidCombination = true;
                }
            }

            Price = price + ProductPrice;
        }

        public bool InvalidCombination { get; set; }
        
        public List<MediaListItem> Media { get; internal set; }
        public string OverlayText { get; internal set; }

        public ProductListItem ToProductListItem()
        {
            return new ProductListItem()
            {
                PID = PID,
                Name = Name,
                Description = Description,
                Url = null,
                Price = Price,
                Media = Media,
                PrimarySilhouetteId = PrimarySilhouetteId,
                OverlayText = OverlayText,
                TaxonString = TaxonString
            };
        }

        public Curation ToCuration()
        {
            return new Curation()
            {
                ProductId = ProductId,
                PID = PID,
                ProductDocumentVersionId = ProductDocumentVersionId, 
                PrimarySilhouetteId = PrimarySilhouetteId,
                Name = Name,
                Description = Description,
                CurationComponents = ComponentIds.Select(c => new CurationComponent { ComponentId = c, PID = PID }).ToList(),
                OverlayText = OverlayText,
                TaxonString = TaxonString
            };
        }

        private void ValidateCombination(string[] pidItems, IReadOnlyDictionary<string, ProductVersion> productVersions)
        {
            InvalidCombination = false;
            if (pidItems.Length > 1)
            {
                var productId = pidItems.First();
                if (!productVersions.ContainsKey(productId))
                {
                    InvalidCombination = true;
                }
            }
            else
            {
                InvalidCombination = true;
            }
        }
    }
}