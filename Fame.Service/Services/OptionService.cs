using Fame.Common.Extensions;
using Fame.Data.Models;
using Fame.Data.Repository;
using Fame.Service.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fame.Service.Services
{
    public class OptionService : BaseService<Option>, IOptionService
    {
        private readonly IRepositories _repositories;

        public OptionService(IRepositories repositories)
            : base(repositories.Option.Value)
        {
            _repositories = repositories;
        }
    }
}
