using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Jasily.DependencyInjection.MethodInvoker
{
    internal class MethodInvoker<T> : IMethodInvoker<T>
    {
        private readonly IServiceProvider serviceProvider;

        private readonly ConcurrentDictionary<MethodInfo, MethodInvoker> invokerMaps
            = new ConcurrentDictionary<MethodInfo, MethodInvoker>();

        public MethodInvoker(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object Invoke(MethodInfo method, T instance = default(T), OverrideArguments arguments = default(OverrideArguments))
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (method.DeclaringType != typeof(T)) throw new InvalidOperationException();
            if (!this.invokerMaps.TryGetValue(method, out var invoker))
            {
                invoker = method.IsStatic ? new StaticMethodInvoker(method) : new InstanceMethodInvoker(method) as MethodInvoker;
                invoker = this.invokerMaps.GetOrAdd(method, invoker);
            }
            return invoker.Invoke(instance, this.serviceProvider, arguments);
        }
    }

    internal abstract class MethodInvoker
    {
        public MethodInvoker(MethodInfo method)
        {
            this.Method = method;
            this.Parameters = this.Method.GetParameters()
                .Select(z => new ParameterInfoDescriptor(z))
                .ToArray();
        }

        public MethodInfo Method { get; }

        public ParameterInfoDescriptor[] Parameters { get; }

        protected static object[] ResolveArguments(MethodInvoker invoker, IServiceProvider provider, OverrideArguments arguments)
        {
            var length = invoker.Parameters.Length;
            var args = new object[length];
            for (var i = 0; i < length; i++)
            {
                var p = invoker.Parameters[i];
                args[i] = p.ResolveArgument(provider, arguments);
            }
            return args;
        }

        protected Expression ResolveArgumentsExpression()
        {
            throw new NotImplementedException();
        }

        public abstract object Invoke(object instance, IServiceProvider provider, OverrideArguments arguments);
    }
}
