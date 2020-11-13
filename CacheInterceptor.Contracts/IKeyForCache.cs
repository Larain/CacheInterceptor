namespace CacheInterceptor.Contracts
{
    public interface IKeyForCache
    {
        string BuildKey();
    }
}