using System;
using System.Linq.Expressions;
using Jasily.DependencyInjection.Internal.CallSites;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Internal
{
    internal class FactoryServiceDescriptor : ServiceDescriptor, IServiceCallSite
    {
        private readonly Func<IServiceProvider, object> factory;

        public FactoryServiceDescriptor([NotNull] Type serviceType, [CanBeNull] string serviceName, ServiceLifetime lifetime,
            [NotNull] Func<IServiceProvider, object> factory)
            : base(serviceType, serviceName, lifetime)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            this.factory = factory;
        }

        public Expression ResolveExpression(ParameterExpression provider)
            => Expression.Invoke(Expression.Constant(this.factory), provider);

        public object ResolveValue(ServiceProvider provider) => this.factory(provider);
    }
}