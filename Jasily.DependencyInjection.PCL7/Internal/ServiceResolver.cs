using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Internal
{
    internal class ServiceResolver : IDisposable
    {
        private static readonly Type EnumerableType = typeof(IEnumerable<>);
        private readonly List<Service> services
            = new List<Service>();
        private readonly Dictionary<Type, TypedServiceEntry> typedServices
            = new Dictionary<Type, TypedServiceEntry>();
        private readonly Dictionary<string, NamedServiceEntry> namedServices
            = new Dictionary<string, NamedServiceEntry>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<Type, TypedServiceEntry> enumerableServices
            = new Dictionary<Type, TypedServiceEntry>();

        public ServiceResolver([NotNull] IEnumerable<IServiceDescriptor> serviceDescriptors)
        {
            Debug.Assert(serviceDescriptors != null);
            this.Init(serviceDescriptors);
        }

        private void Init(IEnumerable<IServiceDescriptor> serviceDescriptors)
        {
            var descriptors = serviceDescriptors.ToArray();
            foreach (var descriptor in descriptors)
            {
                var service = new Service(descriptor);
                this.services.Add(service);
                GetServiceEntry(this.typedServices, service.ServiceType).Add(service);
                GetServiceEntry(this.namedServices, service.ServiceName).Add(service);
            }
        }

        [CanBeNull]
        public ServiceEntry ResolveServiceEntry(ResolveRequest request, ResolveLevel level)
        {
            switch (level)
            {
                case ResolveLevel.TypeAndName:
                case ResolveLevel.Type:
                    if (this.typedServices.TryGetValue(request.ServiceType, out var o))
                        return o;
                    break;

                case ResolveLevel.NameAndType:
                    if (request.ServiceName != string.Empty)
                    {
                        if (this.namedServices.TryGetValue(request.ServiceName, out var e))
                            return e;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }

        public void Dispose()
        {
            foreach (var service in this.services)
            {
                service.Dispose();
            }
            this.services.Clear();
            this.typedServices.Clear();
            this.namedServices.Clear();
        }

        private static TypedServiceEntry GetServiceEntry(Dictionary<Type, TypedServiceEntry> typedServices, Type serviceType)
        {
            TypedServiceEntry typed;
            if (!typedServices.TryGetValue(serviceType, out typed))
            {
                typed = new TypedServiceEntry();
                typedServices.Add(serviceType, typed);
            }
            return typed;
        }

        private static NamedServiceEntry GetServiceEntry(Dictionary<string, NamedServiceEntry> namedServices, string serviceName)
        {
            NamedServiceEntry named;
            if (!namedServices.TryGetValue(serviceName, out named))
            {
                named = new NamedServiceEntry();
                namedServices.Add(serviceName, named);
            }
            return named;
        }
    }
}