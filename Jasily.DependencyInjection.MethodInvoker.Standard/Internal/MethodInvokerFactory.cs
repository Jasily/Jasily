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
#if DEBUG
        /// <summary>
        /// for debugger view. do not use.
        /// </summary>
        private readonly Type _currentType = typeof(T);
#endif
        private readonly HashSet<Type> _baseTypes = new HashSet<Type>();
        private readonly HashSet<ConstructorInfo> _constructors;
        private readonly HashSet<MethodInfo> _methods = new HashSet<MethodInfo>();
        private readonly HashSet<MethodInfo> _genericMethods = new HashSet<MethodInfo>();
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

            // inherit
            var next = typeof(T);
            while ((next = next.GetTypeInfo().BaseType) != null)
            {
                this._baseTypes.Add(next);
            }

            // methods
            var methods = type.GetRuntimeMethods().ToArray();
            foreach (var m in methods.Where(z => !z.IsAbstract))
            {
                if (m.IsGenericMethod)
                {
                    this._genericMethods.Add(m);
                }
                else
                {
                    this._methods.Add(m);
                }
            }

            // properties
            foreach (var (setter, getter) in type.GetRuntimeProperties().Select(z => (z.SetMethod, z.GetMethod)))
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (setter != null) this._methods.Add(setter);
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (getter != null) this._methods.Add(getter);
            }

            // ctors
            this.Constructors = new ReadOnlyCollection<ConstructorInfo>(this._constructors.ToArray());

            // supporteds.
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

            return (BaseInvoker)Activator.CreateInstance(ResolveType(), this, method);
        }

        public BaseInvoker GetMethodInvoker(ConstructorInfo constructor)
        {
            if (!this._constructors.Contains(constructor))
                throw new InvalidOperationException($"Type {typeof(T)} do not contains the constructor.");

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
                if (method.IsGenericMethod && method.ContainsGenericParameters)
                {
                    throw new InvalidOperationException("Method cannot contains generic parameters.");
                }

                var declaringType = method.DeclaringType;
                if (declaringType != typeof(T))
                {
                    if (this._baseTypes.Contains(declaringType))
                    {
                        invoker = ((IInternalMethodInvokerFactory)this.ServiceProvider.GetService(
                            typeof(IMethodInvokerFactory<>).MakeGenericType(method.DeclaringType))
                        ).GetMethodInvoker(method);

                        return this._invokerMaps.GetOrAdd(method, invoker);
                    }
                    throw new InvalidOperationException("Uninherit method.");
                }
                else
                {
                    invoker = this._invokerMaps.GetOrAdd(method, this.CreateInvoker(method));
                }
            }

            if (invoker == null)
            {
                throw new InvalidOperationException($"Type {typeof(T)} do not contains the method.");
            }

            return invoker;
        }

        public IObjectMethodInvoker GetObjectMethodInvoker(MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (method.IsStatic) throw new InvalidOperationException("method is static method.");

            return (IObjectMethodInvoker)this.GetMethodInvoker(method);
        }

        public IInstanceMethodInvoker<T> GetInstanceMethodInvoker([NotNull] MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (method.IsStatic) throw new InvalidOperationException("method is static method.");

            return (IInstanceMethodInvoker<T>)this.GetMethodInvoker(method);
        }

        public IStaticMethodInvoker GetStaticMethodInvoker([NotNull] MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!method.IsStatic) throw new InvalidOperationException("method is instance method.");

            return (IStaticMethodInvoker) this.GetMethodInvoker(method);
        }

        public IStaticMethodInvoker GetConstructorInvoker([NotNull] ConstructorInfo constructor)
        {
            if (constructor == null) throw new ArgumentNullException(nameof(constructor));

            return (IStaticMethodInvoker) this.GetMethodInvoker(constructor);
        }
    }
}
