using Fame.Data.Models;
using Fame.Data.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Fame.Service.Services
{
    public class OptionPriceService : BaseService<OptionPrice>, IOptionPriceService
    {
        private readonly IRepositories _repositories;

        public OptionPriceService(IRepositories repositories)
            : base(repositories.OptionPrice.Value)
        {
            _repositories = repositories;
        }

        public Dictionary<string, int> GetComponentPriceDictionary(int productVersionId, IEnumerable<string> componentIds, string locale)
        {
            return _repositories.OptionPrice.Value.Get()
                    .Where(op => op.Option.ProductVersionId == productVersionId)
                    .Where(op => componentIds.Contains(op.Option.ComponentId))
                    .Where(op => op.LocalisationCode == locale)
                    .Select(c => new { c.Option.ComponentId, Price = c.PriceInMinorUnits })
                    .ToDictionary(c => c.ComponentId, c => c.Price);
        }
    }
}