using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.MethodInvoker
{
    /// <summary>
    /// provide full API explore. 
    /// </summary>
    public struct MethodInvokerProvider
    {
        [CanBeNull]
        private readonly IServiceProvider _serviceProvider;

        internal MethodInvokerProvider(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        [NotNull]
        private IServiceProvider ServiceProvider =>
            this._serviceProvider ?? throw new InvalidOperationException(
                $"Please create from `{nameof(IServiceProvider)}.{nameof(ServiceProviderExtensions.AsMethodInvokerProvider)}()`.");

        /// <summary>
        /// Get <see cref="IMethodInvokerFactory{T}"/> from <see cref="IServiceProvider"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IMethodInvokerFactory<T> GetInvokerFactory<T>()
        {
            var factory = this.ServiceProvider.GetService<IMethodInvokerFactory<T>>();
            this.CheckResult(factory);
            return factory;
        }

        /// <summary>
        /// Get <see cref="IMethodInvokerFactory"/> from <see cref="IServiceProvider"/>.
        /// </summary>
        /// <returns></returns>
        public IMethodInvokerFactory GetInvokerFactory([NotNull] Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var serviceProvider = this.ServiceProvider; // ensure not null.
            var factoryType = typeof(IMethodInvokerFactory<>).MakeGenericType(type);
            var factory = serviceProvider.GetService(factoryType) as IMethodInvokerFactory;
            this.CheckResult(factory);
            return factory;
        }

        private void CheckResult(object factory)
        {
            if (factory == null)
            {
                throw new InvalidOperationException(
                    $"Before get method, please call `{nameof(IServiceCollection)}.{nameof(ServiceCollectionExtensions.AddMethodInvoker)}()`.");
            }
        }

        [NotNull]
        private T GetService<T>()
        {
            var service = this.ServiceProvider.GetService<T>();
            this.CheckResult(service);
            return service;
        }

        /// <summary>
        /// Get <see cref="ISingletonArguments{T}"/> from <see cref="IServiceProvider"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ISingletonArguments<T> GetSingletonArguments<T>()
        {
            return this.GetService<ISingletonArguments<T>>();
        }

        /// <summary>
        /// Get <see cref="IScopedArguments{T}"/> from <see cref="IServiceProvider"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IScopedArguments<T> GetScopedArguments<T>()
        {
            return this.GetService<IScopedArguments<T>>();
        }
    }
}