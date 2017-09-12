using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Jasily.Features.Internal
{
    internal class TypedFeaturesFactory<T> : ITypedFeaturesFactory<T>
        where T : class
    {
        [CanBeNull] private readonly FeaturesOptions<T> _options;
        [CanBeNull] private readonly ITypedFeaturesFactory<T> _baseFactory;

        // ReSharper disable once MemberCanBePrivate.Global
        public TypedFeaturesFactory([NotNull] IFactoryProvider factoryProvider,
            [NotNull] FeaturesOptions<T> options)
        {
            if (factoryProvider == null) throw new ArgumentNullException(nameof(factoryProvider));
            this._options = options; // canbe null.

            if (typeof(T) != typeof(object))
            {
                this._baseFactory = (ITypedFeaturesFactory<T>)factoryProvider.GetFactory(typeof(T).GetTypeInfo().BaseType);
            }
        }

        public TypedFeaturesFactory([NotNull] IFactoryProvider factoryProvider,
            [NotNull] IOptions<FeaturesOptions<T>> options)
            : this(factoryProvider, options.Value)
        {
            // use for IoC inject.
        }

        public TFeature TryCreateFeature<TFeature>(IServiceProvider serviceProvider, T source, bool inherit)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var factory = this._options?.TryGetFactory<TFeature>();
            if (factory != null)
            {
                return factory.Invoke(new FeatureBuildSource<T>(source, serviceProvider));
            }

            if (inherit && this._baseFactory != null)
            {
                return this._baseFactory.TryCreateFeature<TFeature>(serviceProvider, source, true);
            }

            return default(TFeature);
        }

        public object TryCreateFeature(IServiceProvider serviceProvider, Type featureType, T source, bool inherit)
        {
            if (featureType == null) throw new ArgumentNullException(nameof(featureType));
            if (source == null) throw new ArgumentNullException(nameof(source));

            var factory = this._options?.TryGetFactory(featureType);
            if (factory != null)
            {
                return factory.Invoke(new FeatureBuildSource<T>(source, serviceProvider));
            }

            if (inherit && this._baseFactory != null)
            {
                return this._baseFactory.TryCreateFeature(serviceProvider, featureType, source, true);
            }

            return null;
        }
        
        public IEnumerable<object> CreateAllFeatures(IServiceProvider serviceProvider, T source, bool inherit)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var buildSource = new FeatureBuildSource<T>(source, serviceProvider);

            var retVal = this._options?.EnumerateFactorys().Select(z => z(buildSource)) ?? Enumerable.Empty<object>();

            if (inherit && this._baseFactory != null)
            {
                retVal = retVal.Concat(this._baseFactory.CreateAllFeatures(serviceProvider, source, true));
            }

            return retVal;
        }
    }
}