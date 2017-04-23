using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;

namespace Jasily.DependencyInjection.MethodInvoker
{
    internal class ClassMethodInvoker<T> : IMethodInvoker<T>, IMethodInvokerContainer
    {
        private readonly bool isValueType;
        private readonly IServiceProvider serviceProvider;

        private readonly HashSet<MethodInfo> methods = new HashSet<MethodInfo>();
        private readonly ConcurrentDictionary<MethodInfo, MethodInvoker> invokerMaps
            = new ConcurrentDictionary<MethodInfo, MethodInvoker>();

        public ClassMethodInvoker(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            var type = typeof(T);
            this.isValueType = type.GetTypeInfo().IsValueType;
            this.methods = new HashSet<MethodInfo>(type.GetRuntimeMethods());
        }

        public MethodInvoker GetInvoker(MethodInfo method)
        {
            if (!this.invokerMaps.TryGetValue(method, out var invoker))
            {
                if (method.DeclaringType == typeof(T))
                {
                    invoker = method.IsStatic
                        ? new StaticMethodInvoker(method)
                        : new InstanceMethodInvoker<T>(method, this.isValueType) as MethodInvoker;
                    invoker = this.invokerMaps.GetOrAdd(method, invoker);
                }
                else
                {
                    var type = typeof(IMethodInvoker<>).MakeGenericType(method.DeclaringType);
                    var container = (IMethodInvokerContainer) this.serviceProvider.GetService(type);
                    invoker = container.GetInvoker(method);
                }
                invoker = this.invokerMaps.GetOrAdd(method, invoker);
            }
            return invoker;
        }

        public object InvokeInstanceMethod(MethodInfo method, T instance, OverrideArguments arguments = default(OverrideArguments))
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!this.methods.Contains(method)) throw new InvalidOperationException();
            if (method.IsStatic) throw new InvalidOperationException();

            return ((IInstanceMethodInvoker<T>) this.GetInvoker(method)).Invoke(instance, this.serviceProvider, arguments);
        }

        public object InvokeStaticMethod(MethodInfo method, OverrideArguments arguments = default(OverrideArguments))
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!this.methods.Contains(method)) throw new InvalidOperationException();
            if (!method.IsStatic) throw new InvalidOperationException();

            return ((StaticMethodInvoker) this.GetInvoker(method)).Invoke(this.serviceProvider, arguments);
        }
    }
}
