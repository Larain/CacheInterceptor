using CacheInterceptor.Contracts;

namespace CacheInterceptor.Cache.CacheHandler.Implementation
{
    public class MemoryInterceptorCacheHandler : BaseInterceptorCacheHandler
    {
        public MemoryInterceptorCacheHandler(IMemoryCacheManager cacheManager, ICacheExpiration cacheExpiration) : base(cacheManager, cacheExpiration)
        {
        }
    }
}