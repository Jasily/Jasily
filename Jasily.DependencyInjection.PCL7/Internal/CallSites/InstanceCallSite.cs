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
            this.ImplementationInstance = descriptor.Instance;
        }

        public Type ServiceType { get; }

        [CanBeNull]
        public object ImplementationInstance { get; }

        public Expression ResolveExpression(ParameterExpression provider)
            => this.ImplementationInstance == null
                ? Core.Cached.Expression.Null
                : Expression.Constant(this.ImplementationInstance, this.ServiceType);

        public object ResolveValue(ServiceProvider provider) => this.ImplementationInstance;
    }
}