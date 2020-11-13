using CacheInterceptor.Contracts.Data;
using CacheInterceptor.Interfaces;

namespace CacheInterceptor.Cache.Interceptors.Implementation
{
    public class RedisCacheInterceptor : CacheAsyncInterceptor
    {
        public RedisCacheInterceptor(IInterceptorCacheHandler interceptorCacheHandler) : base(interceptorCacheHandler, StorageLocation.Redis)
        {
        }
    }
}