using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Jasily.DependencyInjection.Typed.Internal.Utilitys;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Typed.Internal
{
    internal class GenericTypeMaker : IGenericTypeMaker
    {
        private readonly Type _type;

        private readonly ConcurrentDictionary<TypeArray, Type> _cache
            = new ConcurrentDictionary<TypeArray, Type>(new TypeArrayEqualityComparer());
        private readonly Func<TypeArray, Type> _factory;

        private GenericTypeMaker([NotNull] Type type)
        {
            this._type = type ?? throw new ArgumentNullException(nameof(type));
            this._factory = this.InternalMake;
        }

        private Type InternalMake(TypeArray typeArguments) => this._type.MakeGenericType(typeArguments.Types);

        /// <summary>
        /// make generic type and cache.
        /// </summary>
        /// <param name="typeArguments"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <returns></returns>
        public Type MakeGenericType([NotNull] params Type[] typeArguments)
        {
            if (typeArguments == null) throw new ArgumentNullException(nameof(typeArguments));

            var array = new TypeArray(typeArguments);
            return this._cache.GetOrAdd(array, this._factory);
        }

        private class TypeArrayEqualityComparer : IEqualityComparer<TypeArray>
        {
            public bool Equals(TypeArray x, TypeArray y) => x.Equals(y);

            public int GetHashCode(TypeArray obj) => obj.HashCode;
        }

        private struct TypeArray : IEquatable<TypeArray>
        {
            internal readonly int HashCode;
            public readonly Type[] Types;

            public TypeArray([NotNull] Type[] types)
            {
                this.Types = (Type[]) types.Clone();
                this.HashCode = 0;
                for (var i = 0; i < types.Length; i++)
                {
                    this.HashCode ^= types[i].GetHashCode();
                }
            }

            public bool Equals(TypeArray other)
            {
                if (this.Types.Length != other.Types.Length) return false;
                for (var i = 0; i < this.Types.Length; i++)
                {
                    if (this.Types[i] != other.Types[i]) return false;
                }
                return true;
            }
        }

        public static IReThrowContainer<IGenericTypeMaker> TryCreate([NotNull] Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var typeInfo = type.GetTypeInfo();
            if (!typeInfo.ContainsGenericParameters)
            {
                return DoNotContainsGenericParameters.Instance;
            }

            return new ValueContainer<IGenericTypeMaker>(new GenericTypeMaker(type));
        }

        private class DoNotContainsGenericParameters : IReThrowContainer<IGenericTypeMaker>
        {
            public static readonly DoNotContainsGenericParameters Instance
                = new DoNotContainsGenericParameters();

            public IGenericTypeMaker GetOrThrow() => throw new InvalidOperationException("Type do not contains generic parameters");
        }
    }
}
