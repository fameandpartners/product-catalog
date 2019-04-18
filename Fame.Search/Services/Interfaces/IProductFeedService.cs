using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fame.Data.Models;
using Fame.Search.DTO;

namespace Fame.Search.Services
{
    public interface IProductFeedService
    {
        Task GenerateFeed();
        Task DeleteFeed(int id);
    }
}
