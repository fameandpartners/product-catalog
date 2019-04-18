using System.Threading.Tasks;
using Fame.Service.DTO;

namespace Fame.Service.Services
{
    public interface ISpreeExportService
    {
        Task<SpreeImport> GetExport(int productVersionId);
    }
}
