using System;

namespace CacheInterceptor.Contracts
{
    public interface ICacheExpiration
    {
        TimeSpan Timeout { get; }
    }
}