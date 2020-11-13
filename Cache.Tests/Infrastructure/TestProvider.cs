using System.Threading.Tasks;
using CacheInterceptor.Contracts.Attributes;
using CacheInterceptor.Contracts.Data;

namespace Cache.Tests.Infrastructure
{
    public class TestProvider : ITestProvider
    {
        private readonly IValueStorage _valueStorage;

        public TestProvider(IValueStorage valueStorage)
        {
            _valueStorage = valueStorage;
        }

        public async Task<int> GetInterfaceWithAttributeAsync(string guid)
        {
            return await _valueStorage.GetValueAsync<int>(guid);
        }

        public async Task<int> GetInterfaceWithoutAttributeAsync(string guid)
        {
            return await _valueStorage.GetValueAsync<int>(guid);
        }

        [Cached(60, StorageLocation.Memory)]
        public async Task<int> GetClassWithAttributeAsync(string guid)
        {
            return await _valueStorage.GetValueAsync<int>(guid);
        }

        public async Task<int> GetClassWithoutAttributeAsync(string guid)
        {
            return await _valueStorage.GetValueAsync<int>(guid);
        }

        public int GetInterfaceWithAttribute(string guid)
        {
            return _valueStorage.GetValue<int>(guid);
        }

        public int GetInterfaceWithoutAttribute(string guid)
        {
            return _valueStorage.GetValue<int>(guid);
        }

        [Cached(60, StorageLocation.Memory)]
        public int GetClassWithAttribute(string guid)
        {
            return _valueStorage.GetValue<int>(guid);
        }

        public int GetClassWithoutAttribute(string guid)
        {
            return _valueStorage.GetValue<int>(guid);
        }
    }
}