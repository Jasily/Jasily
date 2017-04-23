using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal class MethodInvokerFactory<T> : IMethodInvokerFactory<T>, IMethodInvokerContainer
    {
        private readonly bool isValueType;
        private readonly IServiceProvider serviceProvider;

        private readonly HashSet<MethodInfo> methods = new HashSet<MethodInfo>();
        private readonly ConcurrentDictionary<MethodInfo, MethodInvoker> invokerMaps
            = new ConcurrentDictionary<MethodInfo, MethodInvoker>();

        public MethodInvokerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            var type = typeof(T);
            this.isValueType = type.GetTypeInfo().IsValueType;
            this.methods = new HashSet<MethodInfo>(type.GetRuntimeMethods());
        }

        public MethodInvoker GetMethodInvoker(MethodInfo method)
        {
            if (!this.invokerMaps.TryGetValue(method, out var invoker))
            {
                if (method.DeclaringType == typeof(T))
                {
                    invoker = method.IsStatic
                        ? new StaticMethodInvoker(this.serviceProvider, method)
                        : new InstanceMethodInvoker<T>(this.serviceProvider, method, this.isValueType) as MethodInvoker;
                    invoker = this.invokerMaps.GetOrAdd(method, invoker);
                }
                else
                {
                    var type = typeof(IMethodInvokerFactory<>).MakeGenericType(method.DeclaringType);
                    var container = (IMethodInvokerContainer) this.serviceProvider.GetService(type);
                    invoker = container.GetMethodInvoker(method);
                }
                invoker = this.invokerMaps.GetOrAdd(method, invoker);
            }
            return invoker;
        }

        public object InvokeInstanceMethod([NotNull] MethodInfo method, [NotNull]  T instance,
            OverrideArguments arguments = default(OverrideArguments))
        {
            return this.GetInstanceMethodInvoker(method).Invoke(instance, arguments);
        }

        public object InvokeStaticMethod([NotNull] MethodInfo method,
            OverrideArguments arguments = default(OverrideArguments))
        {
            return this.GetStaticMethodInvoker(method).Invoke(arguments);
        }

        public IInstanceMethodInvoker<T> GetInstanceMethodInvoker(MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!this.methods.Contains(method)) throw new InvalidOperationException();
            if (method.IsStatic) throw new InvalidOperationException();

            return (IInstanceMethodInvoker<T>)this.GetMethodInvoker(method);
        }

        public IStaticMethodInvoker GetStaticMethodInvoker(MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!this.methods.Contains(method)) throw new InvalidOperationException();
            if (!method.IsStatic) throw new InvalidOperationException();

            return (IStaticMethodInvoker) this.GetMethodInvoker(method);
        }
    }
}
