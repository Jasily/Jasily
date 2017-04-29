using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal abstract class MethodInvoker
    {
#if DEBUG
        protected static readonly bool CompileImmediately = false;
#endif
        protected const int CompileThreshold = 4;

        protected static readonly ParameterExpression ParameterOverrideArguments = Expression.Parameter(typeof(OverrideArguments));
        protected static readonly ParameterExpression ParameterServiceProvider = Expression.Parameter(typeof(IServiceProvider));
        private static readonly MethodInfo MethodResolveArguments;

        static MethodInvoker()
        {
            MethodResolveArguments = typeof(MethodInvoker).GetRuntimeMethods()
                .Where(z => z.Name == nameof(MethodInvoker.ResolveArguments))
                .Where(z => z.GetParameters().Length == 3)
                .Single();
        }

        public MethodInvoker(IInternalMethodInvokerFactory factory, MethodInfo method)
        {
            this.Method = method;
            this.Parameters = this.Method.GetParameters()
                .Select(z => ParameterInfoDescriptor.Build(z))
                .ToArray();
        }

        public MethodInfo Method { get; }

        public ParameterInfoDescriptor[] Parameters { get; }

        private object[] ResolveArguments(IServiceProvider serviceProvider, OverrideArguments arguments)
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
            var p = (ParameterInfoDescriptor<T>) this.Parameters[index];
            return p.ResolveArgumentValue(serviceProvider, arguments);
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

        protected T InvokeMethod<T>(object instance, IServiceProvider serviceProvider, OverrideArguments args)
        {
            var a = this.Parameters.Length == 0 ? null : this.ResolveArguments(serviceProvider, args);

            try
            {
                return (T) this.Method.Invoke(instance, a);
            }
            catch (TargetInvocationException e)
            {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                throw;
            }
        }
    }
}
