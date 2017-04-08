using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Jasily.DependencyInjection.Internal
{
    internal interface IServiceResolver : IDisposable
    {
        ServiceEntry ResolveServiceEntry(ResolveRequest request, ResolveLevel level);
    }

    internal class ServiceResolver : IServiceResolver
    {
        private readonly List<Service> services = new List<Service>();
        private readonly Dictionary<Type, TypedServiceEntry> typedServices
            = new Dictionary<Type, TypedServiceEntry>();
        private readonly Dictionary<string, NamedServiceEntry> namedServices
            = new Dictionary<string, NamedServiceEntry>(StringComparer.OrdinalIgnoreCase);

        public ServiceResolver()
        {
        }

        public ServiceResolver AddRange(IEnumerable<NamedServiceDescriptor> serviceDescriptors)
        {
            Debug.Assert(serviceDescriptors != null);
            var descriptors = serviceDescriptors.ToArray();
            foreach (var descriptor in descriptors)
            {
                var service = new Service(descriptor);
                this.services.Add(service);
                GetServiceEntry(this.typedServices, service.ServiceType).Add(service);
                GetServiceEntry(this.namedServices, service.ServiceName).Add(service);
            }

            return this;
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

    internal class ConcurrentServiceResolver : IServiceResolver
    {
        private readonly ConcurrentDictionary<Type, TypedServiceEntry> typedServices
            = new ConcurrentDictionary<Type, TypedServiceEntry>();
        private readonly ConcurrentDictionary<string, NamedServiceEntry> namedServices
            = new ConcurrentDictionary<string, NamedServiceEntry>(StringComparer.OrdinalIgnoreCase);

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ServiceEntry ResolveServiceEntry(ResolveRequest request, ResolveLevel level)
        {
            throw new NotImplementedException();
        }
    }
}