using CacheInterceptor.Contracts;

namespace CacheInterceptor.Cache.CacheHandler.Implementation
{
    public class RedisInterceptorCacheHandler : BaseInterceptorCacheHandler
    {
        public RedisInterceptorCacheHandler(ICacheManager cacheManager, ICacheExpiration cacheExpiration) : base(cacheManager, cacheExpiration)
        {
        }
    }
}