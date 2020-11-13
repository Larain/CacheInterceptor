using System.Threading.Tasks;
using CacheInterceptor.Contracts.Attributes;
using CacheInterceptor.Contracts.Data;

namespace Cache.Tests.Infrastructure
{
    public interface ITestProvider
    {
        [Cached(60, StorageLocation.Memory)]
        Task<int> GetInterfaceWithAttributeAsync(string guid);

        Task<int> GetInterfaceWithoutAttributeAsync(string guid);

        Task<int> GetClassWithAttributeAsync(string guid);

        Task<int> GetClassWithoutAttributeAsync(string guid);

        [Cached(60, StorageLocation.Memory)]
        int GetInterfaceWithAttribute(string guid);

        int GetInterfaceWithoutAttribute(string guid);

        int GetClassWithAttribute(string guid);

        int GetClassWithoutAttribute(string guid);
    }
}