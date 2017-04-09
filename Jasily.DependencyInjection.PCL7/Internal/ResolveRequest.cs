using System;
using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Internal
{
    internal struct ResolveRequest : IEquatable<ResolveRequest>
    {
        private TypeInfo serviceTypeInfo;

        internal ResolveRequest([NotNull] Type serviceType, [CanBeNull] string serviceName)
        {
            Debug.Assert(serviceType != null);
            this.ServiceType = serviceType;
            this.ServiceName = serviceName ?? string.Empty;
            this.serviceTypeInfo = null;
        }

        [NotNull]
        public Type ServiceType { get; }

        [NotNull]
        public string ServiceName { get; }

        [NotNull]
        public TypeInfo ServiceTypeInfo
            => this.serviceTypeInfo ?? (this.serviceTypeInfo = this.ServiceType.GetTypeInfo());

        public override int GetHashCode()
        {
            return this.ServiceType.GetHashCode() ^ this.ServiceName.GetHashCode();
        }

        public bool Equals(ResolveRequest other)
        {
            return
                this.ServiceType.Equals(other.ServiceType) &&
                this.ServiceName.Equals(other.ServiceName, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj is ResolveRequest rr) return this.Equals(rr);
            return base.Equals(obj);
        }
    }
}