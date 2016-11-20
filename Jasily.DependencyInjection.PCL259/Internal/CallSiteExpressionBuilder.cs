using System;
using System.Linq.Expressions;

namespace Jasily.DependencyInjection.Internal
{
    internal class CallSiteExpressionBuilder
    {
        private static readonly ParameterExpression ProviderParameter = Expression.Parameter(typeof(ServiceProvider));

        private readonly CallSiteRuntimeResolver runtimeResolver;

        public CallSiteExpressionBuilder(CallSiteRuntimeResolver runtimeResolver)
        {
            this.runtimeResolver = runtimeResolver;
        }

        public Func<ServiceProvider, object> Build(IServiceCallSite callSite)
        {
            //if (callSite is SingletonCallSite)
            //{
            //    // If root call site is singleton we can return Func calling
            //    // _runtimeResolver.Resolve directly and avoid Expression generation
            //    return (provider) => _runtimeResolver.Resolve(callSite, provider);
            //}
            return this.BuildExpression(callSite).Compile();
        }

        private Expression<Func<ServiceProvider, object>> BuildExpression(IServiceCallSite callSite)
        {
            var serviceExpression = callSite.ResolveExpression(ProviderParameter);
            return Expression.Lambda<Func<ServiceProvider, object>>(
                serviceExpression, ProviderParameter);
        }
    }
}