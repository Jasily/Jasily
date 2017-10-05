using JetBrains.Annotations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Jasily.TypeMakers
{
    public class GenericTypeMaker : IGenericTypeMaker
    {
        private readonly Type _type;
        private readonly ConcurrentDictionary<TypeArguments, Type> _cache;
        private readonly Func<TypeArguments, Type> _factory;

        [PublicAPI]
        public GenericTypeMaker([NotNull] Type type)
        {
            this._type = type ?? throw new ArgumentNullException(nameof(type));
            if (!type.IsGenericTypeDefinition) throw new InvalidOperationException();
            this._cache = new ConcurrentDictionary<TypeArguments, Type>();
            this._factory = this.InternalMake;
        }

        private Type InternalMake(TypeArguments typeArguments)
        {
            return this._type.MakeGenericType(typeArguments.Types);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeArguments"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// typeArguments is null. -or- Any element of typeArguments is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <see cref="Type.MakeGenericType"/>
        /// </exception>
        public Type MakeGenericType(params Type[] typeArguments)
        {
            var array = new TypeArguments(typeArguments);
            return this._cache.GetOrAdd(array, this._factory);
        }

        private struct TypeArguments : IEquatable<TypeArguments>
        {
            private readonly int _hashCode;
            public readonly Type[] Types;

            public TypeArguments([NotNull] Type[] typeArguments)
            {
                if (typeArguments == null) throw new ArgumentNullException(nameof(typeArguments));

                this.Types = (Type[])typeArguments.Clone();
                this._hashCode = 0;
                for (var i = 0; i < typeArguments.Length; i++)
                {
                    var type = typeArguments[i] ??
                        throw new ArgumentNullException(nameof(typeArguments), "Any element of typeArguments cannot be null.");
                    this._hashCode ^= type.GetHashCode();
                }
            }

            public bool Equals(TypeArguments other)
            {
                if (this.Types.Length != other.Types.Length) return false;
                for (var i = 0; i < this.Types.Length; i++)
                {
                    if (this.Types[i] != other.Types[i]) return false;
                }
                return true;
            }

            public override bool Equals(object obj) => obj is TypeArguments ta && this.Equals(ta);

            public override int GetHashCode() => this._hashCode;
        }
    }
}
