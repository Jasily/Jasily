using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.ExceptionServices;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Internal.CallSites
{
    internal class CreateInstanceCallSite : IServiceCallSite
    {
        private readonly Type implementationType;

        public CreateInstanceCallSite([NotNull] Type implementationType)
        {
            Debug.Assert(implementationType != null);
            this.implementationType = implementationType;
        }


        public Expression ResolveExpression(ParameterExpression provider) => Expression.New(this.implementationType);

        public object ResolveValue(ServiceProvider provider)
        {
            try
            {
                return Activator.CreateInstance(this.implementationType);
            }
            catch (Exception ex) when (ex.InnerException != null)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
    }
}