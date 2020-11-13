using System.Threading.Tasks;

namespace Cache.Tests.Infrastructure
{
    public interface IValueStorage
    {
        Task<T> GetValueAsync<T>(string guid);
        T GetValue<T>(string guid);
    }
}