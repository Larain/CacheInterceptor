using System;
using CacheInterceptor.Contracts.Data;

namespace CacheInterceptor.Contracts.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class CachedAttribute : BaseInterceptorAttribute
    {
        /// <summary>
        /// Cache class methods results in Redis with default cache expiration
        /// </summary>
        /// <param name="methodsResults">Include/Exclude method result from cache</param>
        public CachedAttribute(bool methodsResults = true)
        {
            StorageLocation = StorageLocation.Redis;
            Enabled = methodsResults;
        }

        /// <summary>
        /// Cache class methods in Redis with specified cache expiration (minutes)
        /// </summary>
        /// <param name="durationMinutes">Method result cache duration in minutes. 0 value exclude it from caching</param>
        public CachedAttribute(int durationMinutes)
        {
            Duration = TimeSpan.FromMinutes(durationMinutes);
        }

        /// <summary>
        /// Cache class methods in specified cache storage with default cache expiration
        /// </summary>
        /// <param name="storageLocation">Cache storage location</param>
        public CachedAttribute(StorageLocation storageLocation)
        {
            StorageLocation = storageLocation;
        }

        /// <summary>
        /// Cache class methods in specified cache storage with specified cache expiration (minutes)
        /// </summary>
        /// <param name="durationMinutes">Method result cache duration in minutes. 0 value exclude it from caching</param>
        /// <param name="storageLocation">Cache storage location</param>
        public CachedAttribute(int durationMinutes, StorageLocation storageLocation)
        {
            Duration = TimeSpan.FromMinutes(durationMinutes);
            StorageLocation = storageLocation;
        }

        public TimeSpan? Duration { get; set; }
        public StorageLocation StorageLocation { get; set; }
    }
}