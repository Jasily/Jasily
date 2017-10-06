using System;
using System.Collections.Concurrent;
using System.Reflection;
using Jasily.GenericMakers.Internal;
using JetBrains.Annotations;

namespace Jasily.GenericMakers
{
    public class GenericMethodMaker : IGenericMethodMaker
    {
        private readonly MethodInfo _method;
        private readonly ConcurrentDictionary<TypeArguments, MethodInfo> _cache;
        private readonly Func<TypeArguments, MethodInfo> _factory;

        [PublicAPI]
        public GenericMethodMaker([NotNull] MethodInfo method)
        {
            this._method = method ?? throw new ArgumentNullException(nameof(method));
            if (!method.IsGenericMethodDefinition) throw new InvalidOperationException();
            this._cache = new ConcurrentDictionary<TypeArguments, MethodInfo>();
            this._factory = this.InternalMake;
        }

        private MethodInfo InternalMake(TypeArguments typeArguments)
        {
            return this._method.MakeGenericMethod(typeArguments.Types);
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
        ///     <see cref="MethodInfo.MakeGenericMethod"/>
        /// </exception>
        public MethodInfo MakeGenericMethod(params Type[] typeArguments)
        {
            var array = new TypeArguments(typeArguments);
            return this._cache.GetOrAdd(array, this._factory);
        }
    }
}