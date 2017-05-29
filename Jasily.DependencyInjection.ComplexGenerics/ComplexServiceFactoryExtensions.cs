using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.ComplexGenerics
{
    /// <summary>
    /// extension method for <see cref="IComplexServiceFactory"/>.
    /// </summary>
    public static class ComplexServiceFactoryExtensions
    {
        public static object GetService([NotNull] this IComplexServiceFactory factory, 
            IServiceProvider serviceProvider, Type type)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            var t = factory.GetClosedServiceTypeOrNull(type);
            return t == null ? null : serviceProvider.GetService(t);
        }

        public static IEnumerable<object> GetServices([NotNull] this IComplexServiceFactory factory, 
            IServiceProvider serviceProvider, Type type)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            return factory.GetClosedServiceTypes(type).Select(serviceProvider.GetService);
        }

        public static object GetRequiredService([NotNull] this IComplexServiceFactory factory,
            IServiceProvider serviceProvider, Type type)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            var t = factory.GetClosedServiceTypeOrNull(type);
            return t == null ? throw new InvalidOperationException() : serviceProvider.GetRequiredService(t);
        }

        public static IEnumerable<object> GetRequiredServices([NotNull] this IComplexServiceFactory factory,
            IServiceProvider serviceProvider, Type type)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            return factory.GetClosedServiceTypes(type).Select(serviceProvider.GetRequiredService);
        }
    }
}