using System;
using CacheInterceptor.Contracts;

namespace CacheInterceptor
{
    public class OneHourCacheExpiration : ICacheExpiration
    {
        public TimeSpan Timeout => TimeSpan.FromHours(1);
    }
}