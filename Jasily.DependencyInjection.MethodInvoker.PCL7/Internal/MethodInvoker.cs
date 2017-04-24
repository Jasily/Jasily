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

        protected static readonly ParameterExpression ParameterOverrideArguments = Expression.Parameter(typeof(OverrideArguments));

        public MethodInvoker(IInternalMethodInvokerFactory factory, MethodInfo method)
        {
            this.ServiceProvider = factory.ServiceProvider;
            this.Method = method;
            this.Parameters = this.Method.GetParameters()
                .Select(z => ParameterInfoDescriptor.Build(z))
                .ToArray();
        }

        public MethodInfo Method { get; }

        public IServiceProvider ServiceProvider { get; }

        public ParameterInfoDescriptor[] Parameters { get; }

        protected object[] ResolveArguments(OverrideArguments arguments)
        {
            var length = this.Parameters.Length;
            var args = new object[length];
            for (var i = 0; i < length; i++)
            {
                var p = this.Parameters[i];
                args[i] = p.ResolveArgument(this.ServiceProvider, arguments);
            }
            return args;
        }

        protected Expression[] ResolveArgumentsExpressions()
        {
            var lambda = new Func<OverrideArguments, object[]>(x => this.ResolveArguments(x));
            var args = Expression.Invoke(Expression.Constant(lambda), ParameterOverrideArguments);
            var exps = new Expression[this.Parameters.Length];
            for (var i = 0; i < this.Parameters.Length; i++)
            {
                var item = Expression.ArrayIndex(args, Expression.Constant(i));
                exps[i] = Expression.Convert(item, this.Parameters[i].Parameter.ParameterType);
            }
            return exps;
        }

        protected T InvokeMethod<T>(object instance, OverrideArguments args)
        {
            var a = this.Parameters.Length == 0 ? null : this.ResolveArguments(args);

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
