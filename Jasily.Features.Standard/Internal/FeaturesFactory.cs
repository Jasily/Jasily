using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Features.Internal
{
    internal class FeaturesFactory<T> : IFeaturesFactory<T>
        where T : class
    {
        [CanBeNull] private readonly IServiceProvider _serviceProvider;
        [NotNull] private readonly TypedFeaturesFactory<T> _typedFactory;

        public FeaturesFactory([CanBeNull] IServiceProvider serviceProvider, [NotNull] TypedFeaturesFactory<T> typedFactory)
        {
            this._serviceProvider = serviceProvider;
            this._typedFactory = typedFactory ?? throw new ArgumentNullException(nameof(typedFactory));
        }
        
        public TFeature TryCreateFeature<TFeature>(T source, bool inherit) 
        {
            return this._typedFactory.TryCreateFeature<TFeature>(this._serviceProvider, source, inherit);
        }
        
        public object TryCreateFeature(Type featureType, T source, bool inherit)
        {
            return this._typedFactory.TryCreateFeature(this._serviceProvider, featureType, source, inherit);
        }
        
        public IEnumerable<object> CreateAllFeatures(T source, bool inherit)
        {
            return this._typedFactory.CreateAllFeatures(this._serviceProvider, source, inherit);
        }
    }
}