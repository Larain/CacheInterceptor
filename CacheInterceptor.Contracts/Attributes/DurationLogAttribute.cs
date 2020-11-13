using System;

namespace CacheInterceptor.Contracts.Attributes
{
    // <summary>
    // Log method duration time
    // </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class DurationLogAttribute : BaseInterceptorAttribute
    {
    }
}