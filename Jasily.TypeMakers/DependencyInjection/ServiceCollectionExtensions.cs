using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Jasily.TypeMakers.DependencyInjection
{
    /// <summary>
    /// extension method for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add singleton <see cref="IGenericTypeMakerBucket"/> services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="serviceCollection"/> is null.</exception>
        public static void AddGenericTypeFactory([NotNull] this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.TryAddSingleton<IGenericTypeMakerBucket, GenericTypeMakerBucket>();
        }
    }
}