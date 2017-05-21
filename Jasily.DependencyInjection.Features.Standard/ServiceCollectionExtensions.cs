using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.Features
{
    /// <summary>
    /// extensions for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// add <see cref="IFeaturesFactory{T}"/> to services.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static FeaturesBuilder AddFeatures([NotNull] this IServiceCollection serviceCollection)
        {
            return new FeaturesBuilder(serviceCollection);
        }
    }
}