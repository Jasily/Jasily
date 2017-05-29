using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.ComplexGenerics
{
    /// <summary>
    /// extension method for <see cref="IServiceProvider"/>.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        public static object GetComplexService([NotNull] this IServiceProvider serviceProvider, Type type)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            var factory = serviceProvider.GetService<IComplexServiceFactory>();
            if (factory == null)
            {
                throw new  NotSupportedException();
            }
            return factory.GetService(serviceProvider, type);
        }

        public static T GetComplexService<T>([NotNull] this IServiceProvider serviceProvider)
        {
            return (T) serviceProvider.GetComplexService(typeof(T));
        }

        public static object GetRequiredComplexService([NotNull] this IServiceProvider serviceProvider, Type type)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            var factory = serviceProvider.GetService<IComplexServiceFactory>();
            if (factory == null)
            {
                throw new NotSupportedException();
            }
            return factory.GetRequiredService(serviceProvider, type) ?? throw new NotSupportedException();
        }

        public static T GetRequiredComplexService<T>([NotNull] this IServiceProvider serviceProvider)
        {
            return (T) serviceProvider.GetRequiredComplexService(typeof(T));
        }
    }
}