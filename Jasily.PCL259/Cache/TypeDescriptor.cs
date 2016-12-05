using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

// ReSharper disable StaticMemberInGenericType

namespace Jasily.Cache
{
    public static class TypeDescriptor<T>
    {
        private static MethodInfo[] runtimeMethods;
        private static Dictionary<string, MethodInfo[]> runtimeMethodsMap;

        public static Type Type { get; }

        static TypeDescriptor()
        {
            Type = typeof(T);
        }

        [NotNull]
        [ItemNotNull]
        public static IEnumerable<MethodInfo> RuntimeMethods()
        {
            var methods = runtimeMethods ?? (runtimeMethods = Type.GetRuntimeMethods().ToArray());
            return methods.AsReadOnly();
        }

        [NotNull]
        [ItemNotNull]
        public static IEnumerable<MethodInfo> RuntimeMethods([NotNull] string methodName)
        {
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));

            var map = runtimeMethodsMap ?? (runtimeMethodsMap = RuntimeMethods()
                .GroupBy(z => z.Name)
                .ToDictionary(z => z.Key, z => z.ToArray()));
            var methods = map.GetValueOrDefault(methodName);
            return methods?.AsReadOnly() ?? Enumerable.Empty<MethodInfo>();
        }
    }
}