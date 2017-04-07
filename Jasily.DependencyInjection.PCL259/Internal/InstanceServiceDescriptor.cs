﻿using System;
using System.Linq.Expressions;
using Jasily.DependencyInjection.Internal.CallSites;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Internal
{
    internal class InstanceServiceDescriptor : ServiceDescriptor, IImmutableCallSite
    {
        public InstanceServiceDescriptor([NotNull] Type serviceType, [CanBeNull] string serviceName, [CanBeNull] object instance)
            : base(serviceType, serviceName, ServiceLifetime.Singleton)
        {
            this.ImplementationInstance = instance;
        }

        [CanBeNull]
        public object ImplementationInstance { get; }

        public Expression ResolveExpression(ParameterExpression provider)
            => this.ImplementationInstance == null
                ? Core.Cached.Expression.Null
                : Expression.Constant(this.ImplementationInstance, this.ServiceType);

        public object ResolveValue(ServiceProvider provider) => this.ImplementationInstance;
    }
}