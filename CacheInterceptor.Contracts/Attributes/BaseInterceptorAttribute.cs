using System;

namespace CacheInterceptor.Contracts.Attributes
{
    public abstract class BaseInterceptorAttribute : Attribute
    {
        public bool Enabled { get; set; } = true;
    }
}