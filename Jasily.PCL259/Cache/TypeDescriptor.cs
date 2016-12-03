using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable StaticMemberInGenericType

namespace Jasily.Cache
{
    public static class TypeDescriptor<T>
    {
        private static MethodInfo[] runtimeMethods;

        public static Type Type { get; }

        static TypeDescriptor()
        {
            Type = typeof(T);
        }

        public static IEnumerable<MethodInfo> RuntimeMethods()
        {
            var methods = runtimeMethods ?? (runtimeMethods = Type.GetRuntimeMethods().ToArray());
            return methods.AsEnumerable().AsReadOnly();
        }
    }
}