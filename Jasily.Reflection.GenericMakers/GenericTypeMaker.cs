using System;
using System.Collections.Concurrent;
using Jasily.Reflection.GenericMakers.Internal;
using JetBrains.Annotations;

namespace Jasily.Reflection.GenericMakers
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
    }
}
