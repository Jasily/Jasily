using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.ExceptionServices;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Internal.CallSites
{
    internal class ConstructorCallSite : IServiceCallSite
    {
        private readonly ConstructorInfo constructorInfo;
        private readonly IServiceCallSite[] parameterCallSites;

        public ConstructorCallSite([NotNull] ConstructorInfo constructorInfo, [NotNull] IServiceCallSite[] parameterCallSites)
        {
            Debug.Assert(constructorInfo != null && parameterCallSites != null);
            this.constructorInfo = constructorInfo;
            this.parameterCallSites = parameterCallSites;
        }

        public Expression ResolveExpression(ParameterExpression provider)
        {
            var parameters = this.constructorInfo.GetParameters();
            return Expression.New(this.constructorInfo,
                this.parameterCallSites.Select((c, i) =>
                    Expression.Convert(c.ResolveExpression(provider), parameters[i].ParameterType)));
        }

        public object ResolveValue(ServiceProvider provider)
        {
            var parameterValues = new object[this.parameterCallSites.Length];
            for (var index = 0; index < parameterValues.Length; index++)
            {
                parameterValues[index] = this.parameterCallSites[index].ResolveValue(provider);
            }

            try
            {
                return this.constructorInfo.Invoke(parameterValues);
            }
            catch (Exception ex) when (ex.InnerException != null)
            {
                ex.InnerException.ReThrow();
                throw;
            }
        }
    }
}