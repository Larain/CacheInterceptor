using System;
using System.Threading.Tasks;

namespace CacheInterceptor.Contracts
{
    public interface ICacheManager
    {
        T Get<T>(string key);
        bool Set<T>(string key, T item, TimeSpan? expires);
        Task<T> GetAsync<T>(string key);
        Task<bool> SetAsync<T>(string key, T item, TimeSpan? expires);
    }
}