using System;
using System.Linq.Expressions;
using Jasily.DependencyInjection.Internal.CallSites;

namespace Jasily.DependencyInjection.Internal
{
    internal struct CallSiteExpressionBuilder
    {
        private static readonly ParameterExpression ObjectParameter = Expression.Parameter(typeof(object));
        private static readonly ParameterExpression ProviderParameter = Expression.Parameter(typeof(IServiceProvider));

        public Func<ServiceProvider, object> Build(IServiceCallSite callSite)
        {
            var serviceExpression = callSite.ResolveExpression(ProviderParameter);
            return Expression.Lambda<Func<ServiceProvider, object>>(
                serviceExpression, ProviderParameter).Compile();
        }

        public Action<object, ServiceProvider> Build(IInstanceCallSite callSite)
        {
            var serviceExpression = callSite.ResolveExpression(ObjectParameter, ProviderParameter);
            return Expression.Lambda<Action<object, ServiceProvider>>(
                serviceExpression, ProviderParameter).Compile();
        }

        public Func<object, ServiceProvider, TOut> Build<TOut>(IInstanceCallSite callSite)
        {
            var serviceExpression = callSite.ResolveExpression(ObjectParameter, ProviderParameter);
            return Expression.Lambda<Func<object, ServiceProvider, TOut>>(
                serviceExpression, ProviderParameter).Compile();
        }

        public Func<TIn, ServiceProvider, TOut> Build<TIn, TOut>(IInstanceCallSite callSite)
        {
            var objectParameter = Expression.Parameter(typeof(TIn));
            var serviceExpression = callSite.ResolveExpression(objectParameter, ProviderParameter);
            return Expression.Lambda<Func<TIn, ServiceProvider, TOut>>(
                serviceExpression, ProviderParameter).Compile();
        }
    }
}