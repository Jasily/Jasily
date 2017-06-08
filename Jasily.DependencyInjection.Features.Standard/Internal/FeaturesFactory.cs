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
    internal interface IInternalFeaturesFactory<in T> where T : class
    {
        /// <summary>
        /// Create feature or default(<typeparamref name="TFeature"/>). This will not boxing value type.
        /// </summary>
        /// <typeparam name="TFeature"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <param name="source"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        [CanBeNull]
        TFeature TryCreateFeature<TFeature>([NotNull] IServiceProvider serviceProvider, [NotNull] T source, bool inherit);

        /// <summary>
        /// Create feature or null.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="featureType"></param>
        /// <param name="source"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        [CanBeNull]
        object TryCreateFeature([NotNull] IServiceProvider serviceProvider, [NotNull] Type featureType, [NotNull] T source, bool inherit);

        /// <summary>
        /// Create all registered features for source.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="source"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        [NotNull, ItemNotNull]
        IEnumerable<object> CreateAllFeatures([NotNull] IServiceProvider serviceProvider, [NotNull] T source, bool inherit);
    }

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

            var retVal = this._options.EnumerateFactorys().Select(z => z(buildSource));

            if (inherit && this._baseFactory != null)
            {
                retVal = retVal.Concat(this._baseFactory.CreateAllFeatures(serviceProvider, source, true));
            }

            return retVal;
        }
    }

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