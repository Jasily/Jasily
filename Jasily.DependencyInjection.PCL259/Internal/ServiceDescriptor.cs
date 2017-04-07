using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Internal
{
    internal abstract class ServiceDescriptor : IServiceDescriptor
    {
        protected ServiceDescriptor([NotNull] Type serviceType, [CanBeNull] string serviceName, ServiceLifetime lifetime)
        {
            this.ServiceName = serviceName ?? string.Empty;
            this.ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
            this.Lifetime = lifetime;
        }

        public string ServiceName { get; }
        
        public Type ServiceType { get; }

        public ServiceLifetime Lifetime { get; }
    }
}