using System.Collections.Generic;
using System.Threading.Tasks;
using Fame.Data.Models;
using Fame.Service.DTO;

namespace Fame.Search.Services
{
    public interface ICurationSearchService
    {
        Task<List<ProductListItem>> GetCurationsBySilhouetteAsync(string silhouetteId, string locale);
        Task<List<ProductListItem>> GetCurationsAsync(string[] pids, string locale, bool noMediaForCadImages);
        Task<Curation> UpsertCuration(string pID);
		Task UpsertAllCurations();
        Task<List<string>> ImportCurations(string path);
    }
}