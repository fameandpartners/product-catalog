using Fame.Data.Models;
using Fame.Service.DTO;
using PagedList.Core;

namespace Fame.Service.Services
{
    public interface IFeedMetaService : IBaseService<FeedMeta>
    {
        IPagedList<FeedMetaSummary> GetPage(int page);
        FeedMeta GetLatest();
    }
}