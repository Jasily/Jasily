using System;
using System.Collections.Generic;
using System.Text;
using Jasily.DependencyInjection.MemberInjection.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.MemberInjection
{
    /// <summary>
    /// extension method for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// add <see cref="IMemberInjectorFactory{T}"/> services to <paramref name="serviceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="serviceCollection"/> is null.</exception>
        public static void AddMemberInjection([NotNull] this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddSingleton(typeof(IMemberInjectorFactory<>), typeof(MemberInjectorFactory<>));
            serviceCollection.AddSingleton(typeof(IInstanceInjector<>), typeof(InstanceInjector<>));
            serviceCollection.AddSingleton(typeof(IInjectFieldsSelector<>), typeof(DefaultInjectFieldsSelector<>));
            serviceCollection.AddSingleton(typeof(IInjectPropertiesSelector<>), typeof(DefaultInjectPropertiesSelector<>));
        }
    }
}
