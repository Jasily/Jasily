using System;
using System.Collections.Concurrent;
using Jasily.DependencyInjection.Typed.Internal.Utilitys;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Typed.Internal
{
    internal class GenericTypeFactory : IGenericTypeFactory
    {
        private readonly ConcurrentDictionary<Type, IReThrowContainer<IGenericTypeMaker>> _typeMakersMap
            = new ConcurrentDictionary<Type, IReThrowContainer<IGenericTypeMaker>>();
        private readonly Func<Type, IReThrowContainer<IGenericTypeMaker>> _typeMakerFactory = GenericTypeMaker.TryCreate;

        [NotNull]
        public IGenericTypeMaker GetTypeMaker([NotNull] Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var maker = this._typeMakersMap.GetOrAdd(type, this._typeMakerFactory);
            return maker.GetOrThrow();
        }
    }
}