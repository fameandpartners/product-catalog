using Fame.Data.Models;
using System.Collections.Generic;

namespace Fame.Service.Services
{
    public interface IOptionPriceService : IBaseService<OptionPrice>
    {
        Dictionary<string, int> GetComponentPriceDictionary(int productVersionId, IEnumerable<string> componentIds, string locale);
    }
}