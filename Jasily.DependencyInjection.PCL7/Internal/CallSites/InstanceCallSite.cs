using System;
using System.Linq.Expressions;
using Jasily.DependencyInjection.Internal.CallSites;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.Internal.CallSites
{
    internal sealed class InstanceCallSite : IImmutableCallSite
    {
        public InstanceCallSite(NamedServiceDescriptor descriptor)
        {
            this.ServiceType = descriptor.ServiceType;
            this.Instance = descriptor.Instance;
        }

        public Type ServiceType { get; }

        [CanBeNull]
        public object Instance { get; }

        public Expression ResolveExpression(ParameterExpression provider)
            => this.Instance == null
                ? Core.Cached.Expression.Null
                : Expression.Constant(this.Instance, this.ServiceType);

        public object ResolveValue(ServiceProvider provider) => this.Instance;
    }
}