using CacheInterceptor.Cache;
using CacheInterceptor.Cache.CacheHandler.Implementation;
using CacheInterceptor.Cache.Interceptors;
using CacheInterceptor.Cache.Interceptors.Implementation;
using CacheInterceptor.Contracts;
using CacheInterceptor.Contracts.Data;
using CacheInterceptor.Interfaces;
using CacheInterceptor.Managers;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CacheInterceptor.Installers
{
    public class CacheManagerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMemoryCacheManager>().ImplementedBy<MemoryCacheManager>().LifestyleSingleton());
            container.Register(Component.For<ICacheExpiration>().ImplementedBy<OneHourCacheExpiration>().LifestyleSingleton());

            container.Register(
                Component.For<IInterceptorCacheHandler>().ImplementedBy<RedisInterceptorCacheHandler>(),
                Component.For<IInterceptorCacheHandler>().ImplementedBy<MemoryInterceptorCacheHandler>(),
                Component.For<CacheAsyncInterceptor>().ImplementedBy<RedisCacheInterceptor>()
                    .Named(nameof(StorageLocation) + StorageLocation.Redis).LifestyleSingleton()
                    .DependsOn(Dependency.OnComponent<IInterceptorCacheHandler, RedisInterceptorCacheHandler>()),
                Component.For<CacheAsyncInterceptor>().ImplementedBy<InMemoryCacheInterceptor>()
                    .Named(nameof(StorageLocation) + StorageLocation.Memory).LifestyleSingleton()
                    .DependsOn(Dependency.OnComponent<IInterceptorCacheHandler, MemoryInterceptorCacheHandler>())
            );

            container.AddFacility(new CacheInterceptionFacility());
        }
    }
}
