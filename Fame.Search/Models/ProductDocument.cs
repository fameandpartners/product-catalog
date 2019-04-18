using System.Collections.Generic;
using System.Reflection;

namespace Fame.Search.Models
{
    public class ProductDocument
    {
        public int Index { get; set; }

        public string Id { get; set; }

        public string ProductId { get; set; }

        public int ProductVersionId { get; set; }

        public string ProductName { get; set; }

        public List<string> AggregatedFacets { get; set; }

        public List<string> Collections { get; set; }

        public decimal SortWeight { get; set; }

        public List<ProductVariation> ProductVariations { get; set; }

        public static PropertyInfo WeightProperty => typeof(ProductDocument).GetProperty(nameof(ProductDocument.SortWeight));

        public static PropertyInfo FacetProperty => typeof(ProductDocument).GetProperty(nameof(ProductDocument.AggregatedFacets));

        public static PropertyInfo CollectionsProperty => typeof(ProductDocument).GetProperty(nameof(ProductDocument.Collections));

        public static PropertyInfo IndexProperty => typeof(ProductDocument).GetProperty(nameof(ProductDocument.Index));
    }

    public class ProductVariation
    {
        public List<string> Facets { get; set; }
        
        public string PID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ComponentIdPath { get; set; }

        public string ProductId { get; set; }

        public Dictionary<string, int> Price { get; set; }

        public string PrimarySilhouetteId { get; set; }

        public string PrimarySilhouetteName { get; set; }

        public List<string> OccasionNames { get; set; }

        public string LengthName { get; set; }

        public string ColorName { get; set; }

        public List<string> Taxons { get; set; }
    }
}
