using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Internal
{
    internal abstract class ServiceEntry
    {
        protected static readonly StringComparer StringComparer = StringComparer.OrdinalIgnoreCase;
        protected readonly object SyncRoot = new object();

        public abstract void Add(Service service);

        public abstract Service Resolve([NotNull] ResolveRequest resolveRequest, ResolveLevel level);
    }

    internal class ResolveRequest
    {
        private TypeInfo serviceTypeInfo;

        public ResolveRequest(Type serviceType, string serviceName)
        {
            this.ServiceType = serviceType;
            this.ServiceName = serviceName;
        }

        [NotNull]
        public Type ServiceType { get; }

        [CanBeNull]
        public string ServiceName { get; }

        [NotNull]
        public TypeInfo ServiceTypeInfo
            => this.serviceTypeInfo ?? (this.serviceTypeInfo = this.ServiceType.GetTypeInfo());
    }
}