using System;
using System.Threading.Tasks;
using CacheInterceptor.Contracts.Attributes;
using CacheInterceptor.Contracts.Data;
using CacheInterceptor.Interceptors;
using CacheInterceptor.Interfaces;
using Castle.DynamicProxy;

namespace CacheInterceptor.Cache.Interceptors
{
    public abstract class CacheAsyncInterceptor : BaseAsyncInterceptor<CachedAttribute>
    {
        private readonly StorageLocation _storageLocation;
        private readonly IInterceptorCacheHandler _cacheHandler;

        protected CacheAsyncInterceptor(IInterceptorCacheHandler cacheHandler, StorageLocation storageLocation)
        {
            _cacheHandler = cacheHandler;
            _storageLocation = storageLocation;
        }

        protected override void InterceptInner(IInvocation invocation)
        {
            invocation.Proceed();
        }

        protected override T InterceptInnerWithResult<T>(IInvocation invocation)
        {
            return InterceptWithResult(invocation, duration => _cacheHandler.WrapToCache<T>(invocation, duration));
        }

        protected override async Task InterceptInnerAsync(IInvocation invocation)
        {
            invocation.Proceed();
            await (Task)invocation.ReturnValue;
        }

        protected override Task<T> InterceptInnerWithResultAsync<T>(IInvocation invocation)
        {
            return InterceptWithResult(invocation, duration => _cacheHandler.WrapToCacheAsync<T>(invocation, duration));
        }

        private T InterceptWithResult<T>(IInvocation invocation, Func<TimeSpan?, T> setToCacheFunc)
        {
            var attribute = FindAttribute(invocation);
            if (attribute != null && ShouldBeCached(attribute, invocation))
            {
                return setToCacheFunc(attribute.Duration);
            }
            invocation.Proceed();
            return (T)invocation.ReturnValue;
        }

        private bool ShouldBeCached(CachedAttribute cacheAttribute, IInvocation invocation)
        {
            // due to Redis.Set operation sometimes lasts more then 1.5 sec
            // there is no sense to put something in cache for a such short duration
            if (cacheAttribute.Duration <= TimeSpan.FromSeconds(1))
            {
                throw new ArgumentException($"Invalid cache duration ({cacheAttribute.Duration}) for {invocation.TargetType}, method {invocation.Method.Name}");
            }
            return cacheAttribute.StorageLocation == _storageLocation && cacheAttribute.Enabled;
        }
    }
}