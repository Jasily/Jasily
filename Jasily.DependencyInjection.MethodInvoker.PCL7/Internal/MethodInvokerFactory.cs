using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal class MethodInvokerFactory<T> : IMethodInvokerFactory<T>, IInternalMethodInvokerFactory
    {
        private readonly HashSet<MethodInfo> methods = new HashSet<MethodInfo>();
        private readonly ConcurrentDictionary<MethodInfo, MethodInvoker> invokerMaps
            = new ConcurrentDictionary<MethodInfo, MethodInvoker>();

        public IServiceProvider ServiceProvider { get; }

        public bool IsValueType { get; }

        public MethodInvokerFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            var type = typeof(T);
            this.IsValueType = type.GetTypeInfo().IsValueType;
            this.methods = new HashSet<MethodInfo>(type.GetRuntimeMethods());
        }

        private Type ResolveType(MethodInfo method)
        {
            if (method.IsStatic)
            {
                if (method.ReturnType == typeof(void))
                    return typeof(StaticMethodInvoker);
                else
                    return typeof(StaticMethodInvoker<>).MakeGenericType(method.ReturnType);
            }
            else
            {
                if (method.ReturnType == typeof(void))
                    return typeof(InstanceMethodInvoker<T>);
                else
                    return typeof(InstanceMethodInvoker<,>).MakeGenericType(typeof(T), method.ReturnType);
            }

            throw new NotImplementedException();
        }

        public MethodInvoker GetMethodInvoker(MethodInfo method)
        {
            if (!this.invokerMaps.TryGetValue(method, out var invoker))
            {
                if (method.DeclaringType == typeof(T))
                {
                    invoker = (MethodInvoker) Activator.CreateInstance(this.ResolveType(method), new object[] { this, method });
                    invoker = this.invokerMaps.GetOrAdd(method, invoker);
                }
                else
                {
                    var type = typeof(IMethodInvokerFactory<>).MakeGenericType(method.DeclaringType);
                    var container = (IInternalMethodInvokerFactory) this.ServiceProvider.GetService(type);
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

        public IInstanceMethodInvoker<T> GetInstanceMethodInvoker([NotNull] MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!this.methods.Contains(method)) throw new InvalidOperationException();
            if (method.IsStatic) throw new InvalidOperationException();

            return (IInstanceMethodInvoker<T>)this.GetMethodInvoker(method);
        }

        public IStaticMethodInvoker GetStaticMethodInvoker([NotNull] MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!this.methods.Contains(method)) throw new InvalidOperationException();
            if (!method.IsStatic) throw new InvalidOperationException();

            return (IStaticMethodInvoker) this.GetMethodInvoker(method);
        }
    }
}
