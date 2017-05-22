using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.ComplexGenerics
{
    /// <summary>
    /// extension method for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionServiceExtensions
    {
        public static ComplexGenericsBuilder AddComplexGenerics([NotNull] this IServiceCollection collection)
        {
            return new ComplexGenericsBuilder(collection);
        }
    }
}