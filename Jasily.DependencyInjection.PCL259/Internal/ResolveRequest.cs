using System;
using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Internal
{
    internal class ResolveRequest
    {
        private TypeInfo serviceTypeInfo;

        public ResolveRequest([NotNull] Type serviceType, string serviceName)
        {
            Debug.Assert(serviceType != null);
            this.ServiceType = serviceType;
            this.ServiceName = serviceName ?? string.Empty;
        }

        [NotNull]
        public Type ServiceType { get; }

        [NotNull]
        public string ServiceName { get; }

        [NotNull]
        public TypeInfo ServiceTypeInfo
            => this.serviceTypeInfo ?? (this.serviceTypeInfo = this.ServiceType.GetTypeInfo());
    }
}