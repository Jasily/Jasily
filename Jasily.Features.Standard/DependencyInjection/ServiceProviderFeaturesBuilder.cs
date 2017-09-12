using System;
using Jasily.Features.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Jasily.Features.DependencyInjection
{
    internal class ServiceProviderFeaturesBuilder : IFeaturesBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public IServiceCollection ServiceCollection { get; }

        internal ServiceProviderFeaturesBuilder([NotNull] IServiceCollection serviceCollection)
        {
            this.ServiceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddOptions();
            serviceCollection.TryAddSingleton<IFactoryProvider, ServiceProviderFactoryProvider>();
            serviceCollection.TryAddSingleton(typeof(TypedFeaturesFactory<>));
            serviceCollection.TryAddScoped(typeof(IFeaturesFactory<>), typeof(FeaturesFactory<>));
        }

        /// <summary>
        /// register feature to services.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TFeature"></typeparam>
        /// <param name="factory"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public IFeaturesBuilder RegisterFeature<T, TFeature>(Func<FeatureBuildSource<T>, TFeature> factory, bool overwrite = true)
            where T : class
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            this.ServiceCollection.Configure<FeaturesOptions<T>>(z =>
            {
                z.Register(factory, overwrite);
            });
            return this;
        }
    }
}