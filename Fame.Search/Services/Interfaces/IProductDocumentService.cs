using System.Collections.Generic;
using System.Threading.Tasks;
using Fame.Data.Migrations;
using Fame.Search.DTO;
using Fame.Search.Models;
using Fame.Service.DTO;

namespace Fame.Search.Services
{
    public interface IProductDocumentService
    {
        Task AddAsync(List<ProductDocument> productDocuments, int maxPage = 100);
        Task<ProductResult> GetAsync(SearchArgs searchArgs);
        Task<List<PIDModel>> SetVariationMetaAsync(List<PIDModel> pidModels);
        Task<PIDModel> SetVariationMetaAsync(PIDModel pidModel);
    }
}