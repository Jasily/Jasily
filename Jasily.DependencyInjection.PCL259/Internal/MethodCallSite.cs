using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Jasily.DependencyInjection.Internal
{
    internal class MethodCallSite : IInstanceCallSite, IServiceCallSite
    {
        private readonly Type instanceType;
        private readonly MethodInfo methodInfo;
        private readonly IServiceCallSite[] parameterCallSites;

        public MethodCallSite(Type instanceType, MethodInfo methodInfo, IServiceCallSite[] parameterCallSites)
        {
            Debug.Assert(instanceType != null && methodInfo != null && parameterCallSites != null);
            this.instanceType = instanceType;
            this.methodInfo = methodInfo;
            this.parameterCallSites = parameterCallSites;
        }

        private IEnumerable<Expression> GetParametersExpression(ParameterExpression provider)
        {
            var parameterInfos = this.methodInfo.GetParameters();
            var parameters = this.parameterCallSites.Select(
                (c, i) => Expression.Convert(c.ResolveExpression(provider), parameterInfos[i].ParameterType));
            return parameters;
        }

        public Expression ResolveExpression(ParameterExpression instance, ParameterExpression provider)
        {
            Debug.Assert(!this.methodInfo.IsStatic);
            return Expression.Call(Expression.Convert(instance, this.instanceType),
                this.methodInfo, this.GetParametersExpression(provider));
        }

        public Expression ResolveExpression(ParameterExpression provider)
        {
            Debug.Assert(this.methodInfo.IsStatic);
            return Expression.Call(this.methodInfo, this.GetParametersExpression(provider));
        }

        public object ResolveValue(object instance, ServiceProvider provider)
        {
            var parameterValues = new object[this.parameterCallSites.Length];
            for (var index = 0; index < parameterValues.Length; index++)
            {
                parameterValues[index] = this.parameterCallSites[index].ResolveValue(provider);
            }

            try
            {
                return this.methodInfo.Invoke(instance, parameterValues);
            }
            catch (Exception ex) when (ex.InnerException != null)
            {
                ex.InnerException.ReThrow();
                throw;
            }
        }

        public object ResolveValue(ServiceProvider provider) => this.ResolveValue(null, provider);
    }
}