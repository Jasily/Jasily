using System;
using Jasily.DependencyInjection.MemberInjection.AutoInjection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.MemberInjection
{
    /// <summary>
    /// extension methods for the module.
    /// </summary>
    public static class MemberInjectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="serviceProvider"/> is null.</exception>
        /// <returns></returns>
        public static T GetRequiredServiceAndInject<T>([NotNull] this IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            var service = serviceProvider.GetRequiredService<T>();
            var injector = serviceProvider.GetRequiredService<IInstanceInjector<T>>();
            injector.Inject(service);
            return service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="serviceProvider"/> is null.</exception>
        /// <returns></returns>
        public static T GetServiceAndInject<T>([NotNull] this IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            var service = serviceProvider.GetService<T>();
            if (service != null)
            {
                var injector = serviceProvider.GetRequiredService<IInstanceInjector<T>>();
                injector.Inject(service);
            }
            return service;
        }

        /// <summary>
        /// inject by <see cref="InjectAttribute"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="serviceProvider"/> or <paramref name="instance"/> is null.</exception>
        public static void InjectMember<T>([NotNull] this IServiceProvider serviceProvider, [NotNull] T instance)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var injector = serviceProvider.GetRequiredService<IInstanceInjector<T>>();
            injector.Inject(instance);
        }
    }
}
