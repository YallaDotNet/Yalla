using System;
using System.Linq;

namespace Yalla.Configuration
{
    static class TypeExtensions
    {
        public static T GetCustomAttribute<T>(this Type type)
            where T : Attribute
        {
            return type.GetCustomAttributes(typeof(T), false)
                .Cast<T>()
                .SingleOrDefault();
        }
    }
}
