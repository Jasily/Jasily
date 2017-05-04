using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal class MethodInvokerFactory<T> : IMethodInvokerFactory<T>, IInternalMethodInvokerFactory
    {
        private readonly HashSet<ConstructorInfo> _constructors;
        private readonly HashSet<MethodInfo> _methods;
        private readonly ConcurrentDictionary<MethodBase, BaseInvoker> _invokerMaps
            = new ConcurrentDictionary<MethodBase, BaseInvoker>();

        public IServiceProvider ServiceProvider { get; }

        public bool IsValueType { get; }

        public MethodInvokerFactory([NotNull] IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            var type = typeof(T);
            this.IsValueType = type.GetTypeInfo().IsValueType;           
            this._constructors = new HashSet<ConstructorInfo>(type.GetTypeInfo().DeclaredConstructors);
            this._methods = new HashSet<MethodInfo>(type.GetRuntimeMethods());
            foreach (var (setter, getter) in type.GetRuntimeProperties().Select(z => (z.SetMethod, z.GetMethod)))
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (setter != null) this._methods.Add(setter);
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (getter != null) this._methods.Add(getter);
            }
            this.Constructors = new ReadOnlyCollection<ConstructorInfo>(this._constructors.ToArray());
            this.Methods = new ReadOnlyCollection<MethodInfo>(this._methods.ToArray());
        }

        public IReadOnlyList<ConstructorInfo> Constructors { get; }

        public IReadOnlyList<MethodInfo> Methods { get; }

        private BaseInvoker CreateInvoker(ConstructorInfo constructor)
        {
            var type = typeof(ConstructorInvoker<>).MakeGenericType(typeof(T));

            return (BaseInvoker)Activator.CreateInstance(type, this, constructor);
        }

        private BaseInvoker CreateInvoker(MethodInfo method)
        {
            Type ResolveType()
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

            return (BaseInvoker)Activator.CreateInstance(ResolveType(), new object[] { this, method });
        }

        public BaseInvoker GetMethodInvoker(ConstructorInfo constructor)
        {
            if (!this._invokerMaps.TryGetValue(constructor, out var invoker))
            {
                invoker = this.CreateInvoker(constructor);
                invoker = this._invokerMaps.GetOrAdd(constructor, invoker);
            }
            return invoker;
        }

        public BaseInvoker GetMethodInvoker(MethodInfo method)
        {
            if (!this._invokerMaps.TryGetValue(method, out var invoker))
            {
                invoker = this._invokerMaps.GetOrAdd(method, this.CreateInvoker(method));
            }
            return invoker;
        }

        public IInstanceMethodInvoker<T> GetInstanceMethodInvoker([NotNull] MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!this._methods.Contains(method))
                throw new InvalidOperationException($"type {typeof(T)} does not contains the method.");
            if (method.IsStatic) throw new InvalidOperationException($"method is static method.");

            return (IInstanceMethodInvoker<T>)this.GetMethodInvoker(method);
        }

        public IStaticMethodInvoker GetStaticMethodInvoker([NotNull] MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!this._methods.Contains(method))
                throw new InvalidOperationException($"type {typeof(T)} does not contains the method.");
            if (!method.IsStatic) throw new InvalidOperationException($"method is instance method.");

            return (IStaticMethodInvoker) this.GetMethodInvoker(method);
        }

        public IStaticMethodInvoker GetConstructorInvoker([NotNull] ConstructorInfo constructor)
        {
            if (constructor == null) throw new ArgumentNullException(nameof(constructor));
            if (!this._constructors.Contains(constructor))
                throw new InvalidOperationException($"type {typeof(T)} does not contains the constructor.");

            return (IStaticMethodInvoker) this.GetMethodInvoker(constructor);
        }
    }
}
