using Fame.Data.Models;
using Fame.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using PagedList.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fame.Service.Services
{
	public class CurationService : BaseService<Curation>, ICurationService
    {
        private readonly IRepositories _repositories;
        private readonly ICurationRepository _curationRepo;
        private readonly IFacetRepository _facetRepo;
        private readonly IDistributedCache _distributedCache;
        private readonly ICurationMediaRepository _curationMediaRepo;
        private readonly ICurationMediaVariantRepository _curationMediaVariantRepo;
        private readonly ICurationComponentRepository _curationComponentRepo;

        public CurationService(IRepositories repositories,
            IDistributedCache distributedCache)
            : base(repositories.Curation.Value)
        {
            _repositories = repositories;
            _curationRepo = repositories.Curation.Value;
            _facetRepo = repositories.Facet.Value;
            _distributedCache = distributedCache;
            _curationMediaRepo = repositories.CurationMedia.Value;
            _curationMediaVariantRepo = repositories.CurationMediaVariant.Value;
            _curationComponentRepo = repositories.CurationComponent.Value;
        }

        public IPagedList<Curation> GetCurations(int page, int pageSize)
        {
            return _curationRepo.Get()
                .Include(c => c.Product)
                .Include(c => c.Facet)
                .Include(c => c.CurationComponents)
					.ThenInclude(c => c.Component)
                .Include(c => c.Facet)
                .Include(c => c.Media)
                    .ThenInclude(c => c.CurationMediaVariants)
                .ToPagedList(page, pageSize);
        }

        public Task<Curation> GetCuration(string pid)
        {
            return _curationRepo.Get()
                .Include(c => c.Product)
                .Include(c => c.Facet)
                .Include(c => c.CurationComponents)
					.ThenInclude(c => c.Component)
                .Include(c => c.Facet)
                .Include(c => c.Media)
                    .ThenInclude(c => c.CurationMediaVariants)
				.SingleOrDefaultAsync(c => c.PID == pid);
        }

		public void RemovePrimarySilhouetteIds()
		{
			foreach (var item in _curationRepo.Get())
			{
				item.PrimarySilhouetteId = null;
				Update(item);
			}			
		}

		public IEnumerable<string> GetPIDs()
		{
			return _curationRepo.Get().Select(c => c.PID);
		}

        public string[] GetPIDsBySilhouette(string silhouetteId)
        {
            return _curationRepo.Get().Where(c => c.PrimarySilhouetteId == silhouetteId && c.Media.Any()).OrderBy(p => p.PID).Select(p => p.PID).ToArray();
        }

        public void Delete(string pid)
        {
            _curationMediaVariantRepo.DeleteWhere(cmv => cmv.CurationMedia.PID == pid);
            _curationMediaRepo.DeleteWhere(cmv => cmv.PID == pid);
            _curationComponentRepo.DeleteWhere(cmv => cmv.PID == pid);
            _curationRepo.DeleteWhere(cmv => cmv.PID == pid);
        }
    }
}
