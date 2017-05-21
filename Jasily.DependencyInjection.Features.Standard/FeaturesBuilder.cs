using System;
using Jasily.DependencyInjection.Features.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Jasily.DependencyInjection.Features
{
    /// <summary>
    /// 
    /// </summary>
    public class FeaturesBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public IServiceCollection ServiceCollection { get; }

        internal FeaturesBuilder([NotNull] IServiceCollection serviceCollection)
        {
            this.ServiceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddOptions();
            serviceCollection.TryAddSingleton(typeof(IFeaturesFactory<>), typeof(FeaturesFactory<>));
        }

        /// <summary>
        /// register feature to services.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TFeature"></typeparam>
        /// <param name="factory"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public FeaturesBuilder RegisterFeature<T, TFeature>([NotNull] Func<FeatureBuildSource<T>, TFeature> factory, bool overwrite = true)
            where T : class
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            this.ServiceCollection.Configure<FeaturesFactoryOptions<T>>(z =>
            {
                z.Register(factory, overwrite);
            });
            return this;
        }
    }
}