using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Jasily.DependencyInjection.Features.Internal
{
    internal class InternalFeaturesFactory<T> : IInternalFeaturesFactory<T>
        where T : class
    {
        [NotNull] private readonly FeaturesFactoryOptions<T> _options;
        [CanBeNull] private readonly IInternalFeaturesFactory<T> _baseFactory;

        public InternalFeaturesFactory([NotNull] IServiceProvider serviceProvider, [NotNull] IOptions<FeaturesFactoryOptions<T>> options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            this._options = options.Value;

            if (typeof(T) != typeof(object) && !typeof(T).GetTypeInfo().IsValueType)
            {
                this._baseFactory = (IInternalFeaturesFactory<T>)serviceProvider.GetRequiredService(
                    typeof(InternalFeaturesFactory<>).MakeGenericType(typeof(T).GetTypeInfo().BaseType));
            }
        }

        [CanBeNull]
        public TFeature TryCreateFeature<TFeature>([NotNull] IServiceProvider serviceProvider, [NotNull] T source, bool inherit)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var factory = this._options.TryGetFactory<TFeature>();
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

        [CanBeNull]
        public object TryCreateFeature([NotNull] IServiceProvider serviceProvider, [NotNull] Type featureType, [NotNull] T source, bool inherit)
        {
            if (featureType == null) throw new ArgumentNullException(nameof(featureType));
            if (source == null) throw new ArgumentNullException(nameof(source));

            var factory = this._options.TryGetFactory(featureType);
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

        [NotNull, ItemNotNull]
        public IEnumerable<object> CreateAllFeatures([NotNull] IServiceProvider serviceProvider, [NotNull] T source, bool inherit)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var buildSource = new FeatureBuildSource<T>(source, serviceProvider);

            var retVal = Enumerable.Select(this._options.EnumerateFactorys(), z => z(buildSource));

            if (inherit && this._baseFactory != null)
            {
                retVal = retVal.Concat<object>(this._baseFactory.CreateAllFeatures(serviceProvider, source, true));
            }

            return retVal;
        }
    }
}