using System.Collections.Generic;
using Fame.Data.Models;
using Fame.Service.DTO;

namespace Fame.Service.Services
{
    public interface IProductCombinationService
	{
		IEnumerable<ProductCombination> GetCombinationsForOption(Option option, ProductVersion productVersion);
		IEnumerable<ProductCombination> GetCombinationsForProduct(ProductVersion productVersion, bool indexedOnly = false);
		IEnumerable<ProductCombination> GetCombinationsForComponentTypes(List<ComponentType> componentTypes, ProductVersion productVersion, Option[] startingPath, bool indexedOnly = false);
	}
}
