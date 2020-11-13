using CacheInterceptor.Contracts.Data;
using CacheInterceptor.Interfaces;

namespace CacheInterceptor.Cache.Interceptors.Implementation
{
    public class InMemoryCacheInterceptor : CacheAsyncInterceptor
    {
        public InMemoryCacheInterceptor(IInterceptorCacheHandler interceptorCacheHandler) : base(interceptorCacheHandler, StorageLocation.Memory)
        {
        }
    }
}