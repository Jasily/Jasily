using Jasily.Features.DependencyInjection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.Features.DependencyInjection
{
    /// <summary>
    /// extensions for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Features module to services.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IFeaturesBuilder AddFeatures([NotNull] this IServiceCollection serviceCollection)
        {
            return new ServiceProviderFeaturesBuilder(serviceCollection);
        }
    }
}