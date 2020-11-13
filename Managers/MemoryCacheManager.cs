using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using CacheInterceptor.Contracts;

namespace CacheInterceptor.Managers
{
    public class MemoryCacheManager : IMemoryCacheManager
    {
        private readonly ICacheExpiration _cacheExpiration;
        private readonly MemoryCache _cache;

        public MemoryCacheManager(ICacheExpiration cacheExpiration)
        {
            _cacheExpiration = cacheExpiration;
            _cache = MemoryCache.Default;
        }

        public Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult((T)_cache.Get(key));
        }

        public T Get<T>(string key)
        {
            return (T)_cache.Get(key);
        }

        public bool Set<T>(string key, T item, TimeSpan? expires = null)
        {
            _cache.Set(key, item, DateTimeOffset.Now.Add(expires ?? _cacheExpiration.Timeout));
            return true;
        }

        public Task<bool> SetAsync<T>(string key, T item, TimeSpan? expires)
        {
            _cache.Set(key, item, DateTimeOffset.Now.Add(expires ?? _cacheExpiration.Timeout));
            return Task.FromResult(true);
        }
    }
}