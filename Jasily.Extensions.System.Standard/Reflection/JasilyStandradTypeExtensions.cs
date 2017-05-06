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
        public static Type FastMakeGenericType([NotNull] this Type genericType, [NotNull] params Type[] typeArguments)
        {
            if (genericType == null) throw new ArgumentNullException(nameof(genericType));
            if (typeArguments == null) throw new ArgumentNullException(nameof(typeArguments));
            
            var array = new TypeArray(genericType, typeArguments);
            return Cache.TryGetValue(array, out var result)
                ? result
                : Cache.GetOrAdd(array, genericType.MakeGenericType(typeArguments));
        }

        private class TypeArrayEqualityComparer : IEqualityComparer<TypeArray>
        {
            public bool Equals(TypeArray x, TypeArray y) => x.Equals(y);

            public int GetHashCode(TypeArray obj) => obj.HashCode;
        }

        private struct TypeArray : IEquatable<TypeArray>
        {
            internal readonly int HashCode;
            private readonly Type[] _types;

            public TypeArray([NotNull] Type type, [NotNull] Type[] types)
            {
                this._types = new Type[types.Length + 1];
                this._types[0] = type;
                this.HashCode = type.GetHashCode();
                for (var i = 0; i < types.Length; i++)
                {
                    this.HashCode ^= types[i].GetHashCode();
                    this._types[i + 1] = types[i];
                }
            }

            public bool Equals(TypeArray other)
            {
                if (this._types.Length != other._types.Length) return false;
                for (var i = 0; i < this._types.Length; i++)
                {
                    if (this._types[i] != other._types[i]) return false;
                }
                return true;
            }
        }
    }
}
