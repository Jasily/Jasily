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
        [NotNull]
        public static IComplexServiceFactory GetComplexServiceFactory([NotNull] this IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            var factory = serviceProvider.GetService<IComplexServiceFactory>();
            return factory ?? throw new InvalidOperationException(
                $"Before get {nameof(IComplexServiceFactory)}, " +
                $"please call `{nameof(IServiceCollection)}.{nameof(ServiceCollectionExtensions.AddComplexGenerics)}()`.");
        }

        public static object GetComplexService([NotNull] this IServiceProvider serviceProvider, Type type)
        {
            var factory = serviceProvider.GetComplexServiceFactory();
            return factory.GetService(serviceProvider, type);
        }

        public static object GetRequiredComplexService([NotNull] this IServiceProvider serviceProvider, Type type)
        {
            var factory = serviceProvider.GetComplexServiceFactory();
            return factory.GetRequiredService(serviceProvider, type) ?? throw new NotSupportedException();
        }

        public static T GetComplexService<T>([NotNull] this IServiceProvider serviceProvider)
        {
            return (T)serviceProvider.GetComplexService(typeof(T));
        }

        public static T GetRequiredComplexService<T>([NotNull] this IServiceProvider serviceProvider)
        {
            return (T) serviceProvider.GetRequiredComplexService(typeof(T));
        }
    }
}