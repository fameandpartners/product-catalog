using Fame.Common;
using Microsoft.Extensions.Options;
using ServiceStack.Redis;
using System.Collections.Generic;
using System.Linq;

namespace Fame.Service.Services
{
    public class CacheService : ICacheService
    {
        private readonly FameConfig _fameConfig;

        public CacheService(IOptions<FameConfig> fameConfig)
        {
            _fameConfig = fameConfig.Value;
        }

        public void DeleteAll()
        {
            DeleteWithPrefix(null);
        }

        public void DeleteWithPrefix(string prefix)
        {
            var manager = new RedisManagerPool($"{_fameConfig.Cache.Server}:{_fameConfig.Cache.Port}");
            using (var client = manager.GetClient())
            {
                var keys = client.GetAllKeys().Where(s => s.StartsWith($"{_fameConfig.Cache.InstanceName}{prefix}"));
                client.RemoveAll(keys);
            }
        }

        public IEnumerable<string> GetPage(int pageSize = 10, string prefix = null)
        {
            var manager = new RedisManagerPool($"{_fameConfig.Cache.Server}:{_fameConfig.Cache.Port}");
            using (var client = manager.GetClient())
            {
                return client.ScanAllKeys().ToList();
            }
        }
    }
}