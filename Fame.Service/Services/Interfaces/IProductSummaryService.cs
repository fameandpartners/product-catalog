using Fame.Service.DTO;
using System.Threading.Tasks;

namespace Fame.Service.Services
{
    public interface IProductSummaryService
    {
        ProductSummary GetProductSummary(string id, string localisationCode);
    }
}