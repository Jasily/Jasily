using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.Extensions.System.Reflection
{
    public static class TypeInfoExtensions
    {
        public static bool CanInstantiation([NotNull] this TypeInfo type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return !type.IsAbstract && !type.IsInterface && !type.IsGenericTypeDefinition;
        }

        public static bool IsStaticClass([NotNull] this TypeInfo type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return type.IsAbstract && type.IsSealed;
        }
    }
}