﻿using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Jasily.DependencyInjection.MethodInvoker.Internal;

namespace Jasily.DependencyInjection.MethodInvoker
{
    /// <summary>
    /// extension method for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// add <see cref="MethodInvoker"/> services to <paramref name="serviceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="serviceCollection"/> is null.</exception>
        public static void AddMethodInvoker([NotNull] this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

            if (serviceCollection.Any(z => z.ServiceType == typeof(IMethodInvokerFactory<>))) return;

            serviceCollection.AddSingleton(typeof(IMethodInvokerFactory<>), typeof(MethodInvokerFactory<>));
            serviceCollection.AddSingleton(typeof(ISingletonArguments<>), typeof(ConcurrentArguments<>));
            serviceCollection.AddScoped(typeof(IScopedArguments<>), typeof(ConcurrentArguments<>));
            serviceCollection.AddSingleton(typeof(IArgumentResolver<>), typeof(ArgumentResolver<>));
        }

        /// <summary>
        /// add <see cref="MethodInvoker"/> services to <paramref name="serviceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="serviceCollection"/> is null.</exception>
        [Obsolete("use `AddMethodInvoker`")]
        public static void UseMethodInvoker([NotNull] this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMethodInvoker();
        }
    }
}
