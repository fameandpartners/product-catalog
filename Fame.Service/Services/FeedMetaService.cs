using Fame.Common;
using Fame.Data.Models;
using Fame.Data.Repository;
using Fame.Service.DTO;
using Microsoft.Extensions.Options;
using PagedList.Core;
using System.Linq;

namespace Fame.Service.Services
{
    public class FeedMetaService : BaseService<FeedMeta>, IFeedMetaService
    {
        private readonly FameConfig _fameConfig;
        private readonly IFeedMetaRepository _feedMetaRepo;

        public FeedMetaService(IRepositories repositories,
            IOptions<FameConfig> fameConfig)
            : base(repositories.FeedMeta.Value)
        {
            _fameConfig = fameConfig.Value;
            _feedMetaRepo = repositories.FeedMeta.Value;
        }

        public FeedMeta GetLatest()
        {
            return _feedMetaRepo.Get().OrderByDescending(f => f.Id).FirstOrDefault();
        }

        public IPagedList<FeedMetaSummary> GetPage(int page)
        {
            return _feedMetaRepo.Get().OrderByDescending(f => f.Id).Select(f => new FeedMetaSummary { CreatedOn = f.CreatedOn, Url = f.FullPath(_fameConfig.Document.Url), Id = f.Id }).ToPagedList(page, 20);
        }
    }
}