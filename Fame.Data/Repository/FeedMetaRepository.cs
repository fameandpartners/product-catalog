using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public class FeedMetaRepository : BaseRepository<FeedMeta>, IFeedMetaRepository
    {
        public FeedMetaRepository(FameContext context) : base(context)
        {
        }
    }
}