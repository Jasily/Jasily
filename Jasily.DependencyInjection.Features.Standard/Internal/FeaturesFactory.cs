using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Features.Internal
{
    internal class FeaturesFactory<T> : IFeaturesFactory<T>
        where T : class
    {
        [NotNull] private readonly IServiceProvider _serviceProvider;
        [CanBeNull] private readonly InternalFeaturesFactory<T> _internalFactory;

        public FeaturesFactory([NotNull] IServiceProvider serviceProvider, [NotNull] InternalFeaturesFactory<T> internalFactory)
        {
            this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this._internalFactory = internalFactory ?? throw new ArgumentNullException(nameof(internalFactory));
        }

        [CanBeNull]
        public TFeature TryCreateFeature<TFeature>([NotNull] T source, bool inherit) 
        {
            return this._internalFactory.TryCreateFeature<TFeature>(this._serviceProvider, source, inherit);
        }

        [CanBeNull]
        public object TryCreateFeature([NotNull] Type featureType, [NotNull] T source, bool inherit)
        {
            return this._internalFactory.TryCreateFeature(this._serviceProvider, featureType, source, inherit);
        }

        [NotNull, ItemNotNull]
        public IEnumerable<object> CreateAllFeatures([NotNull] T source, bool inherit)
        {
            return this._internalFactory.CreateAllFeatures(this._serviceProvider, source, inherit);
        }
    }
}