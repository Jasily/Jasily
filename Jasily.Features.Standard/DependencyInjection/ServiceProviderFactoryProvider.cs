using System;
using Jasily.Features.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.Features.DependencyInjection
{
    internal class ServiceProviderFactoryProvider : IFactoryProvider
    {
        [NotNull] private readonly IServiceProvider _serviceProvider;

        public ServiceProviderFactoryProvider([NotNull] IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public object GetFactory(Type type)
        {
            var factoryType = typeof(TypedFeaturesFactory<>).MakeGenericType(type);
            return this._serviceProvider.GetRequiredService(factoryType);
        }
    }
}