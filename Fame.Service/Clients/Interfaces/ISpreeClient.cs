using System.Collections.Generic;
using System.Threading.Tasks;
using Fame.Service.DTO;

namespace Fame.Service.Clients.Interfaces
{
    public interface ISpreeClient
    {
        Task ImportProduct(ICollection<SpreeImport> data);
    }
}
