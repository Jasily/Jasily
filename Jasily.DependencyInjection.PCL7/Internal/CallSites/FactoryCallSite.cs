using System.Diagnostics;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jasily.DependencyInjection.Internal.CallSites
{
    internal class FactoryCallSite : IServiceCallSite
    {
        private readonly Func<IServiceProvider, object> factory;

        public FactoryCallSite(NamedServiceDescriptor descriptor)
        {
            Debug.Assert(descriptor.Mode == NamedServiceDescriptor.ImplementationMode.Factory);
            this.factory = descriptor.Factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public Expression ResolveExpression(ParameterExpression provider)
            => Expression.Invoke(Expression.Constant(this.factory), provider);

        public object ResolveValue(ServiceProvider provider) => this.factory(provider);
    }
}
