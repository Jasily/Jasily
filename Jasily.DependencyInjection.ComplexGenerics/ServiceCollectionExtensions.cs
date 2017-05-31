using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.ComplexService
{
    /// <summary>
    /// extension method for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static ComplexServiceBuilder AddComplexService([NotNull] this IServiceCollection collection)
        {
            return new ComplexServiceBuilder(collection);
        }
    }
}