using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Fame.Common.Extensions
{
    public static partial class DistributedCachingExtensions
    {

        public static async Task<T> GetOrSetAsync<T>(this IDistributedCache distributedCache, CacheKey cacheKey, Func<Task<T>> action, DistributedCacheEntryOptions options = default(DistributedCacheEntryOptions), CancellationToken token = default(CancellationToken))
        {
            var key = cacheKey.Key;
            var cached = await distributedCache.GetAsync<T>(key, token);

            if (cached != null) return cached;

            var value = await action.Invoke();
            if(value!=null)//为空时无需写入缓存
                await distributedCache.SetAsync(key, value, options ?? new DistributedCacheEntryOptions(), token);

            return value;
        }

        public static T GetOrSet<T>(this IDistributedCache distributedCache, CacheKey cacheKey, Func<T> action, DistributedCacheEntryOptions options = default(DistributedCacheEntryOptions))
        {
            var key = cacheKey.Key;
            var cached = distributedCache.Get<T>(key);

            if (cached != null) return cached;

            var value = action.Invoke();
            if (value != null)//为空时不写入缓存
                distributedCache.Set(key, ToByteArray(value), options ?? new DistributedCacheEntryOptions());

            return value;
        }

        #region Private Methods

        private static async Task SetAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options = default(DistributedCacheEntryOptions), CancellationToken token = default(CancellationToken))
        {
            await distributedCache.SetAsync(key, ToByteArray(value), options, token);
        }

        private static T Get<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default(CancellationToken))
        {
            var result = distributedCache.Get(key);
            return FromByteArray<T>(result);
        }

        private static async Task<T> GetAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default(CancellationToken))
        {
            var result = await distributedCache.GetAsync(key, token);
            return FromByteArray<T>(result);
        }

        private static byte[] ToByteArray(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        private static T FromByteArray<T>(byte[] byteArray)
        {
            if (byteArray == null)
            {
                return default(T);
            }

            try
            {
                var binaryFormatter = new BinaryFormatter();
                using (var memoryStream = new MemoryStream(byteArray))
                {
                    return (T)binaryFormatter.Deserialize(memoryStream);
                }
            }
            catch
            {
                // Something went wrong deserialising so just return the default
                return default(T);
            }
        }

        #endregion
    }
}
