using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;

namespace CacheInterceptor.Installers
{
    internal static class AttributeFinder
    {
        public static ICollection<TAttribute> FindAttributeInClassOrInterface<TAttribute>(this Type type) where TAttribute : Attribute
        {
            var interfaces = type.GetInterfaces();

            var attributes = type.GetAttributes<TAttribute>() // class has attribute
                .Union(interfaces.SelectMany(m => m.GetAttributes<TAttribute>())) // interface has attribute
                .Union(type.GetMethods().SelectMany(m => m.GetAttributes<TAttribute>())) // class method has attribute
                .Union(interfaces.SelectMany(i => i.GetMethods().SelectMany(m => m.GetAttributes<TAttribute>()))) // interface method has attribute
                .ToList();

            return attributes;
        }
    }
}