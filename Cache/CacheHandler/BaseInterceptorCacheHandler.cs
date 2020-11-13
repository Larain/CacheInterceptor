using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CacheInterceptor.Contracts;
using CacheInterceptor.Interfaces;
using Castle.DynamicProxy;

namespace CacheInterceptor.Cache.CacheHandler
{
    public abstract class BaseInterceptorCacheHandler : IInterceptorCacheHandler
    {
        private readonly ICacheManager _cacheManager;
        private readonly ICacheExpiration _cacheExpiration;

        private static readonly HashSet<Type> AvailableKeyTypes = new HashSet<Type> { typeof(decimal), typeof(decimal?), typeof(string), typeof(DateTime), typeof(DateTime?) };

        protected BaseInterceptorCacheHandler(ICacheManager cacheManager, ICacheExpiration cacheExpiration)
        {
            _cacheManager = cacheManager;
            _cacheExpiration = cacheExpiration;
        }

        public async Task<T> WrapToCacheAsync<T>(IInvocation invocation, TimeSpan? duration)
        {
            var cacheKey = BuildCacheKeyFrom(invocation);

            //try get the return value from the cache provider
            var cachedValue = _cacheManager.Get<CacheItemWrapper<T>>(cacheKey);

            if (cachedValue != null)
            {
                return await Task.FromResult(cachedValue.Item).ConfigureAwait(false);
            }

            //call the intercepted method
            invocation.Proceed();

            var resultTask = (Task<T>) invocation.ReturnValue;
            var setInCacheTask = resultTask.ContinueWith(t =>
            {
                var result = new CacheItemWrapper<T>(t.Result);
                if (!t.IsFaulted && t.Result != null)
                {
                    _cacheManager.Set(cacheKey, result, duration ?? _cacheExpiration.Timeout);
                }
            });
            await setInCacheTask;
            return await resultTask.ConfigureAwait(false);
        }

        public T WrapToCache<T>(IInvocation invocation, TimeSpan? duration)
        {
            var cacheKey = BuildCacheKeyFrom(invocation);

            //try get the return value from the cache provider
            var cachedValue = _cacheManager.Get<CacheItemWrapper<T>>(cacheKey);

            if (cachedValue != null)
            {
                return cachedValue.Item;
            }

            //call the intercepted method
            invocation.Proceed();

            if (invocation.ReturnValue == null) return default(T);
            var cachedItem = new CacheItemWrapper<T>((T) invocation.ReturnValue);
            _cacheManager.Set(cacheKey, cachedItem, duration ?? _cacheExpiration.Timeout);
            return cachedItem.Item;
        }

        #region private

        private static string BuildCacheKeyFrom(IInvocation invocation)
        {
            var argsList = new List<string>();
            if (invocation.GenericArguments != null)
            {
                argsList.AddRange(invocation.GenericArguments.Select(g => g.Name));
            }

            foreach (var argument in invocation.Arguments)
            {
                var type = argument?.GetType();

                if (type == null)
                {
                    argsList.Add("null");
                }
                else if (AvailableKeyTypes.Contains(type) || type.IsPrimitive || type.IsEnum)
                {
                    argsList.Add(Convert.ToString(argument, CultureInfo.InvariantCulture));
                }
                else if (type.IsAssignableFrom(typeof(IKeyForCache)))
                {
                    argsList.Add(((IKeyForCache)argument).BuildKey());
                }
                else
                {
                    throw new ArgumentException($"All arguments of method {invocation.Method.Name} of class {invocation.TargetType.Name}" +
                                                $" must be value type or implement {nameof(IKeyForCache)} interface");
                }
            }

            var methodName = $"{invocation.TargetType.Name}.{invocation.Method.Name.Replace("Async", "")}";
            var argsString = string.Join(" + ", argsList);

            var cacheKey = $"<{methodName}: {argsString}>";
            return cacheKey;
        }

        #endregion
        
        // wrap item in class to be able check value type for null
        private class CacheItemWrapper<T>
        {
            public CacheItemWrapper(T item)
            {
                Item = item;
            }
            public T Item { get; }
        }

    }
}