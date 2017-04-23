using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;

namespace Jasily.DependencyInjection.MethodInvoker
{
    internal class MethodInvoker<T> : IMethodInvoker<T>
    {
        private readonly IServiceProvider serviceProvider;

        private readonly HashSet<MethodInfo> methods = new HashSet<MethodInfo>();
        private readonly ConcurrentDictionary<MethodInfo, MethodInvoker> invokerMaps
            = new ConcurrentDictionary<MethodInfo, MethodInvoker>();

        public MethodInvoker(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.methods = new HashSet<MethodInfo>(typeof(T).GetRuntimeMethods());
        }

        public object InvokeInstanceMethod(MethodInfo method, T instance, OverrideArguments arguments = default(OverrideArguments))
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!this.methods.Contains(method)) throw new InvalidOperationException();
            if (method.IsStatic) throw new InvalidOperationException();
            if (!this.invokerMaps.TryGetValue(method, out var invoker))
            {
                invoker = this.invokerMaps.GetOrAdd(method, new InstanceMethodInvoker(method));
            }
            return invoker.Invoke(instance, this.serviceProvider, arguments);
        }

        public object InvokeStaticMethod(MethodInfo method, OverrideArguments arguments = default(OverrideArguments))
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!this.methods.Contains(method)) throw new InvalidOperationException();
            if (!method.IsStatic) throw new InvalidOperationException();
            if (!this.invokerMaps.TryGetValue(method, out var invoker))
            {
                invoker = this.invokerMaps.GetOrAdd(method, new StaticMethodInvoker(method));
            }
            return invoker.Invoke(null, this.serviceProvider, arguments);
        }
    }

    internal abstract class MethodInvoker
    {
        protected static readonly ParameterExpression ParameterServiceProvider = Expression.Parameter(typeof(IServiceProvider));
        protected static readonly ParameterExpression ParameterOverrideArguments = Expression.Parameter(typeof(OverrideArguments));

        public MethodInvoker(MethodInfo method)
        {
            this.Method = method;
            this.Parameters = this.Method.GetParameters()
                .Select(z => ParameterInfoDescriptor.Build(z))
                .ToArray();
        }

        public MethodInfo Method { get; }

        public ParameterInfoDescriptor[] Parameters { get; }

        protected object[] ResolveArguments(IServiceProvider provider, OverrideArguments arguments)
        {
            var length = this.Parameters.Length;
            var args = new object[length];
            for (var i = 0; i < length; i++)
            {
                var p = this.Parameters[i];
                args[i] = p.ResolveArgument(provider, arguments);
            }
            return args;
        }

        protected Expression[] ResolveArgumentsExpressions()
        {
            var lambda = new Func<IServiceProvider, OverrideArguments, object[]>((z, x) => this.ResolveArguments(z, x));
            var args = Expression.Invoke(Expression.Constant(lambda), ParameterServiceProvider, ParameterOverrideArguments);
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

        public abstract object Invoke(object instance, IServiceProvider provider, OverrideArguments arguments);
    }
}
