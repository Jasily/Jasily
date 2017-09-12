using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.Features.DependencyInjection
{
    /// <summary>
    /// extensions for <see cref="IServiceProvider"/>.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// get <see cref="IFeaturesFactory{T}"/> to services.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        [NotNull]
        public static IFeaturesFactory<T> GetFeaturesFactory<T>([NotNull] this IServiceProvider serviceProvider)
            where T : class
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            return serviceProvider.GetService<IFeaturesFactory<T>>() ??
                   throw new NotSupportedException(
                       $"Before call {nameof(GetFeaturesFactory)}, " +
                       $"you should call {nameof(IServiceCollection)}.{nameof(ServiceCollectionExtensions.AddFeatures)}()");
        }
    }
}