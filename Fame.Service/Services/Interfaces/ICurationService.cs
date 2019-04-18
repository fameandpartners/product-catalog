using Fame.Data.Models;
using PagedList.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fame.Service.Services
{
    public interface ICurationService : IBaseService<Curation>
    {
        IPagedList<Curation> GetCurations(int page, int pageSize);
        Task<Curation> GetCuration(string pid);
        void RemovePrimarySilhouetteIds();
        IEnumerable<string> GetPIDs();
        string[] GetPIDsBySilhouette(string silhouetteId);
        void Delete(string pid);
    }
}