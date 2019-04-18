using Fame.Data.Models;
using System.Collections.Generic;

namespace Fame.Service.Services
{
    public interface ICacheService
    {
        void DeleteWithPrefix(string prefix);
        void DeleteAll();
        IEnumerable<string> GetPage(int pageSize = 10, string prefix = null);
    }
}