using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal abstract class MethodInvoker
    {
        protected static readonly ParameterExpression ParameterOverrideArguments = Expression.Parameter(typeof(OverrideArguments));

        public MethodInvoker(IServiceProvider serviceProvider, MethodInfo method)
        {
            this.ServiceProvider = serviceProvider;
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

        protected Expression ResolveBodyExpressions(Expression body)
        {
            if (this.Method.ReturnType == typeof(void) || this.Method.ReturnType == typeof(object))
            {
                return body;
            }
            else
            {
                return Expression.Convert(body, typeof(object));
            }
        }
    }
}
