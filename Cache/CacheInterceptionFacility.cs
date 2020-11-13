using System.Linq;
using CacheInterceptor.Contracts.Attributes;
using CacheInterceptor.Contracts.Data;
using CacheInterceptor.Installers;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;

namespace CacheInterceptor.Cache
{
    public class CacheInterceptionFacility : AbstractFacility
    {
        protected override void Init()
        {
            Kernel.ComponentRegistered += Kernel_ComponentRegistered;
        }

        private static void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            var attributes = handler.ComponentModel.Implementation.FindAttributeInClassOrInterface<CachedAttribute>();

            if (!attributes.Any()) return;

            var filtered = attributes.Select(a => a.StorageLocation).Distinct();
            foreach (var storageLocation in filtered)
            {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(nameof(StorageLocation) + storageLocation));
            }
        }
    }
}