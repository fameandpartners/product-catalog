using System.Collections.Generic;
using Fame.Data.Models;

namespace Fame.Service.DTO
{
    public class ProductCombination
    {
        public ProductVersion ProductVersion { get; set; }
        public List<Option> Options { get; set; }

        public override bool Equals(object obj)
        {
            var combination = obj as ProductCombination;
            return combination != null &&
                   EqualityComparer<ProductVersion>.Default.Equals(ProductVersion, combination.ProductVersion) &&
                   EqualityComparer<IEnumerable<Option>>.Default.Equals(Options, combination.Options);
        }

        public override int GetHashCode()
        {
            var hashCode = -137777307;
            hashCode = hashCode * -1521134295 + EqualityComparer<ProductVersion>.Default.GetHashCode(ProductVersion);
            hashCode = hashCode * -1521134295 + EqualityComparer<IEnumerable<Option>>.Default.GetHashCode(Options);
            return hashCode;
        }
    }

    public class AggregatedProductCombination
    {
        public string Id { get; set; }
        public List<ProductCombination> ProductCombinations { get; set; }
    }
}
