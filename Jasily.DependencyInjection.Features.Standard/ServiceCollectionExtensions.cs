using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.Features
{
    public static class ServiceCollectionExtensions
    {
        public static FeaturesBuilder AddFeatures([NotNull] this IServiceCollection serviceCollection)
        {
            return new FeaturesBuilder(serviceCollection);
        }
    }
}