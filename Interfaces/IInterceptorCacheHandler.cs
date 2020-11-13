using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace CacheInterceptor.Interfaces
{
    public interface IInterceptorCacheHandler
    {
        Task<T> WrapToCacheAsync<T>(IInvocation invocation, TimeSpan? duration);
        T WrapToCache<T>(IInvocation invocation, TimeSpan? duration);
    }
}