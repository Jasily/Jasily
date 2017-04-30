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
        private readonly HashSet<MethodBase> methods = new HashSet<MethodBase>();
        private readonly ConcurrentDictionary<MethodBase, BaseInvoker> invokerMaps
            = new ConcurrentDictionary<MethodBase, BaseInvoker>();

        public IServiceProvider ServiceProvider { get; }

        public bool IsValueType { get; }

        public MethodInvokerFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            var type = typeof(T);
            this.IsValueType = type.GetTypeInfo().IsValueType;
            this.methods = new HashSet<MethodBase>(type.GetRuntimeMethods());
            foreach (var ctor in type.GetTypeInfo().DeclaredConstructors)
            {
                this.methods.Add(ctor);
            }
        }

        private BaseInvoker CreateInvoker(MethodBase function)
        {
            if (function is MethodInfo method)
            {
                return (BaseInvoker)Activator.CreateInstance(this.ResolveType(method), new object[] { this, method });
            }
            else if (function is ConstructorInfo constructor)
            {
                return (BaseInvoker)Activator.CreateInstance(this.ResolveType(constructor), new object[] { this, constructor });
            }

            throw new NotSupportedException();
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
        }

        private Type ResolveType(ConstructorInfo constructor)
        {
            return typeof(ConstructorInvoker<>).MakeGenericType(typeof(T));
        }

        public BaseInvoker GetMethodInvoker(MethodBase method)
        {
            if (!this.invokerMaps.TryGetValue(method, out var invoker))
            {
                if (method.DeclaringType == typeof(T))
                {
                    invoker = this.CreateInvoker(method);
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

        public IInstanceMethodInvoker<T> GetInstanceMethodInvoker([NotNull] MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!this.methods.Contains(method))
                throw new InvalidOperationException($"type {typeof(T)} does not contains the method.");
            if (method.IsStatic) throw new InvalidOperationException($"method is static method.");

            return (IInstanceMethodInvoker<T>)this.GetMethodInvoker(method);
        }

        public IStaticMethodInvoker GetStaticMethodInvoker([NotNull] MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!this.methods.Contains(method))
                throw new InvalidOperationException($"type {typeof(T)} does not contains the method.");
            if (!method.IsStatic) throw new InvalidOperationException($"method is instance method.");

            return (IStaticMethodInvoker) this.GetMethodInvoker(method);
        }

        public IStaticMethodInvoker GetConstructorInvoker([NotNull] ConstructorInfo constructor)
        {
            if (constructor == null) throw new ArgumentNullException(nameof(constructor));
            if (!this.methods.Contains(constructor))
                throw new InvalidOperationException($"type {typeof(T)} does not contains the constructor.");

            return (IStaticMethodInvoker) this.GetMethodInvoker(constructor);
        }
    }
}
