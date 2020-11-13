using System.Threading.Tasks;

namespace Cache.Tests.Infrastructure
{
    public class ValueStorageWrapper : IValueStorage
    {
        private IValueStorage _realStorage;
        public async Task<T> GetValueAsync<T>(string guid)
        {
            return await _realStorage.GetValueAsync<T>(guid);
        }

        public T GetValue<T>(string guid)
        {
            return _realStorage.GetValue<T>(guid);
        }

        public void SetMock(IValueStorage mock)
        {
            _realStorage = mock;
        }
    }
}