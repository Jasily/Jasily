using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Jasily.DependencyInjection.Features.Internal
{
    internal class FeaturesFactory<T> : IFeaturesFactory<T>
        where T : class
    {
        [NotNull] private readonly IServiceProvider _serviceProvider;
        [NotNull] private readonly FeaturesFactoryOptions<T> _options;
        [CanBeNull] private readonly IFeaturesFactory<T> _baseFactory;

        public FeaturesFactory([NotNull] IServiceProvider serviceProvider, [NotNull] IOptions<FeaturesFactoryOptions<T>> options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            this._options = options.Value;
            this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            if (typeof(T) != typeof(object))
            {
                this._baseFactory = (IFeaturesFactory<T>) serviceProvider.GetRequiredService(
                    typeof(IFeaturesFactory<>).MakeGenericType(typeof(T).GetTypeInfo().BaseType));
            }
        }

        [CanBeNull]
        public TFeature TryCreateFeature<TFeature>([NotNull] T source, bool inherit) 
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var factory = this._options.TryGetFactory<TFeature>();
            if (factory != null)
            {
                return factory.Invoke(new FeatureBuildSource<T>(source, this._serviceProvider));
            }

            if (inherit && this._baseFactory != null)
            {
                return this._baseFactory.TryCreateFeature<TFeature>(source, true);
            }

            return default(TFeature);
        }

        [CanBeNull]
        public object TryCreateFeature([NotNull] Type featureType, [NotNull] T source, bool inherit)
        {
            if (featureType == null) throw new ArgumentNullException(nameof(featureType));
            if (source == null) throw new ArgumentNullException(nameof(source));

            var factory = this._options.TryGetFactory(featureType);
            if (factory != null)
            {
                return factory.Invoke(new FeatureBuildSource<T>(source, this._serviceProvider));
            }

            if (inherit && this._baseFactory != null)
            {
                return this._baseFactory.TryCreateFeature(featureType, source, true);
            }

            return null;
        }

        [NotNull, ItemNotNull]
        public IEnumerable<object> CreateAllFeatures([NotNull] T source, bool inherit)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var buildSource = new FeatureBuildSource<T>(source, this._serviceProvider);

            var retVal = this._options.EnumerateFactorys().Select(z => z(buildSource));

            if (inherit && this._baseFactory != null)
            {
                retVal = retVal.Concat(this._baseFactory.CreateAllFeatures(source, true));
            }

            return retVal;
        }
    }
}