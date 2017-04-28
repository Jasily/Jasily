using System;
using System.Collections.Generic;
using System.Text;
using Jasily.DependencyInjection.MemberInjection.AutoInjection;
using Jasily.DependencyInjection.MemberInjection.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.MemberInjection
{
    /// <summary>
    /// extension method for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionServiceExtensions
    {
        /// <summary>
        /// add <see cref="IMemberInjectorFactory{}"/> services to <paramref name="serviceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="serviceCollection"/> is null.</exception>
        public static void UseMemberInjector([NotNull] this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddSingleton(typeof(IMemberInjectorFactory<>), typeof(MemberInjectorFactory<>));
        }

        /// <summary>
        /// add <see cref="IInstanceInjector{}"/> services to <paramref name="serviceCollection"/>.
        /// require call: <see cref="UseMemberInjector(IServiceCollection)"/>.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="serviceCollection"/> is null.</exception>
        public static void UseInstanceInjector([NotNull] this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            
            serviceCollection.AddSingleton(typeof(IInstanceInjector<>), typeof(InstanceInjector<>));
        }
    }
}
