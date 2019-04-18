using System;
using System.Collections.Generic;
using System.Linq;
using Fame.Data.Models;

namespace Fame.Service.Services
{

    public partial class ImportService
    {
        private class ProductImport : IDataImport<ProductImport>
        {
            public string ProductId { get; set; }
            public string Title { get; set; }
            public string RelatedRenderComponentGroupType { get; set; }
            public List<string> Groups { get; set; }
            public bool Index { get; set; }
            public ProductType ProductType { get; set; }
            public int AUD { get; set; }
            public int USD { get; set; }
            public string Factory { get; set; }
            public List<string> CollectionIds { get; set; }
            public PreviewType PreviewType { get; set; }
            public bool Active { get; set; }
            public string DropBoxAssetFolder { get; set; }
            public string DropName { get; set; }
            public bool DisableLayering { get; set; }

            public ProductImport FromCsv(string csvLine)
            {
                var values = csvLine.Split(',');
                return new ProductImport
                {
                    ProductId = values[0],
                    Title = values[1],
                    RelatedRenderComponentGroupType = values[2],
                    Groups = values[3].Split("|", StringSplitOptions.RemoveEmptyEntries).ToList(),
                    Index = bool.Parse(values[4]),
                    ProductType = (ProductType)Enum.Parse(typeof(ProductType), values[5]),
                    AUD = int.TryParse(values[6], out var i) ? i * 100 : 0,
                    USD = int.TryParse(values[7], out i) ? i * 100 : 0,
                    Factory = values[8],
                    CollectionIds = values[9].Split("|", StringSplitOptions.RemoveEmptyEntries).ToList(),
                    PreviewType = (PreviewType)Enum.Parse(typeof(PreviewType), values[10]),
                    Active = bool.Parse(values[11]),
                    DropBoxAssetFolder = values[12],
                    DropName = values[13],
                    DisableLayering = bool.Parse(values[14])
                };
            }
            
            public string TabTitle => "Product";
        }
    }
}