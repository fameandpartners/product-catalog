using System.Threading.Tasks;

namespace Fame.Search.Services
{
    public interface IIndexService
    {
        Task FullIndex();
        Task<bool> DropIndex();
    }
}