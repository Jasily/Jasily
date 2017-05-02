using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace System.Reflection
{
    /// <summary>
    /// extension methods for <see cref="Type"/>.
    /// </summary>
    public static class JasilyStandradTypeExtensions
    {
        private static readonly ConcurrentDictionary<TypeArray, Type> Cache
            = new ConcurrentDictionary<TypeArray, Type>(new TypeArrayEqualityComparer());

        /// <summary>
        /// make generic type and cache.
        /// </summary>
        /// <param name="genericType"></param>
        /// <param name="typeArguments"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <returns></returns>
        public static Type FastMakeGenericType([NotNull] Type genericType, [NotNull] params Type[] typeArguments)
        {
            if (genericType == null) throw new ArgumentNullException(nameof(genericType));
            if (typeArguments == null) throw new ArgumentNullException(nameof(typeArguments));

            var array = new TypeArray(genericType, typeArguments);
            return Cache.TryGetValue(array, out var result) ? result : Cache.GetOrAdd(array, genericType.MakeGenericType(typeArguments));
        }

        private class TypeArrayEqualityComparer : IEqualityComparer<TypeArray>
        {
            public bool Equals(TypeArray x, TypeArray y)
                => x.Types.Length == y.Types.Length && x.Types.SequenceEqual(y.Types);

            public int GetHashCode(TypeArray obj)
                => obj.Types.Aggregate(0, (current, t) => current ^ t.GetHashCode());
        }

        private struct TypeArray
        {
            internal readonly Type[] Types;

            public TypeArray(Type type, Type[] types)
            {
                this.Types = new Type[types.Length + 1];
                this.Types[0] = type;
                for (var i = 0; i < types.Length; i++)
                {
                    this.Types[i + 1] = types[i];
                }
            }
        }
    }
}
