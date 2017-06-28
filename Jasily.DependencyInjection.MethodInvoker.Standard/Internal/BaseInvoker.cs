using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal abstract class BaseInvoker
    {
#if DEBUG
        protected static readonly bool CompileImmediately = true;
#endif
        protected const int CompileThreshold = 4;

        protected static readonly ParameterExpression ParameterOverrideArguments = Expression.Parameter(typeof(OverrideArguments));
        protected static readonly ParameterExpression ParameterServiceProvider = Expression.Parameter(typeof(IServiceProvider));
        private static readonly MethodInfo MethodResolveArguments;

        static BaseInvoker()
        {
            MethodResolveArguments = typeof(BaseInvoker)
                .GetRuntimeMethods()
                .Where(z => z.Name == nameof(BaseInvoker.ResolveArguments))
                .Single(z => z.GetParameters().Length == 3);
        }


        protected BaseInvoker(IInternalMethodInvokerFactory factory, MethodBase method)
        {
            this.Parameters = method.GetParameters()
                .Select(ParameterInfoDescriptor.Build)
                .ToArray();
        }

        protected ParameterInfoDescriptor[] Parameters { get; }

        protected object[] ResolveArguments(IServiceProvider serviceProvider, OverrideArguments arguments)
        {
            var length = this.Parameters.Length;
            var args = new object[length];
            for (var i = 0; i < length; i++)
            {
                var p = this.Parameters[i];
                args[i] = p.ResolveArgumentObject(serviceProvider, arguments);
            }
            return args;
        }

        private T ResolveArguments<T>(IServiceProvider serviceProvider, OverrideArguments arguments, int index)
        {
            return ((ParameterInfoDescriptor<T>)this.Parameters[index]).ResolveArgumentValue(serviceProvider, arguments);
        }

        protected Expression[] ResolveArgumentsExpressions()
        {
            var exps = new Expression[this.Parameters.Length];
            for (var i = 0; i < this.Parameters.Length; i++)
            {
                var p = this.Parameters[i];
                var m = MethodResolveArguments.MakeGenericMethod(p.Parameter.ParameterType);
                exps[i] = Expression.Call(Expression.Constant(this), m,
                    ParameterServiceProvider, ParameterOverrideArguments, Expression.Constant(i));
            }
            return exps;
        }
    }
}
