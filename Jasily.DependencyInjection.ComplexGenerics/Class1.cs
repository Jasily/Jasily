using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.ComplexGenerics
{
    /// <summary>
    /// extension method for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionServiceExtensions
    {
        public static ResolverBuilder UseComplexGenericsResolver([NotNull] this IServiceCollection collection)
        {
            return new ResolverBuilder(collection ?? throw new ArgumentNullException(nameof(collection)));
        }
    }

    public struct ResolverBuilder
    {
        private readonly IServiceCollection _collection;
        private readonly ComplexGenericsResolverProvider _resolverProvider;

        internal ResolverBuilder(IServiceCollection collection)
        {
            this._collection = collection;
            var resolverProvider = collection
                .SingleOrDefault(z => z.ImplementationInstance is ComplexGenericsResolverProvider)
                ?.ImplementationInstance as ComplexGenericsResolverProvider;
            if (resolverProvider == null)
            {
                resolverProvider = new ComplexGenericsResolverProvider();
                collection.AddSingleton(resolverProvider);
            }
            this._resolverProvider = resolverProvider;
        }

        private void CommonAdd([NotNull] Type serviceType, [NotNull] Type implementationType)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            if (this._collection == null || this._resolverProvider == null) throw new InvalidOperationException();

            var resolver = new ComplexGenericsResolver(serviceType, implementationType);
            this._resolverProvider.AddResolver(resolver);

            var genericTypeDefinition = serviceType.GetGenericTypeDefinition();
            this._collection.AddSingleton(genericTypeDefinition, sp =>
            {
                var provider = sp.GetRequiredService<ComplexGenericsResolverProvider>();
                throw new NotImplementedException();
                var closedImplType = provider.GetClosedImplTypeOrNull(null /* not null ! */);
                return closedImplType == null ? null : sp.GetService(closedImplType);
            });
        }

        public ResolverBuilder AddSingleton([NotNull] Type serviceType, [NotNull] Type implementationType)
        {
            this.CommonAdd(serviceType, implementationType);
            this._collection.AddSingleton(implementationType);
            return this;
        }

        public ResolverBuilder AddScoped([NotNull] Type serviceType, [NotNull] Type implementationType)
        {
            this.CommonAdd(serviceType, implementationType);
            this._collection.AddScoped(implementationType);
            return this;
        }

        public ResolverBuilder AddTransient([NotNull] Type serviceType, [NotNull] Type implementationType)
        {
            this.CommonAdd(serviceType, implementationType);
            this._collection.AddTransient(implementationType);
            return this;
        }
    }

    internal class ComplexGenericsResolverProvider
    {
        private readonly Dictionary<Type, List<ComplexGenericsResolver>> _map = new Dictionary<Type, List<ComplexGenericsResolver>>();

        public void AddResolver(ComplexGenericsResolver resolver)
        {
            throw new NotImplementedException();
        }

        public Type GetClosedImplTypeOrNull(Type closedServiceType)
        {
            throw new NotImplementedException();
        }
    }

    internal class ComplexGenericsResolver
    {
        private readonly ConcurrentDictionary<Type, Type> _map;
        private readonly GenericTypeTree _tree;

        public ComplexGenericsResolver([NotNull] Type serviceType, [NotNull] Type implementationType)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));

            this.ServiceTypeGenericTypeDefinition = serviceType.GetGenericTypeDefinition();

            this._tree = new GenericTypeTree(serviceType, implementationType);
            this._map = new ConcurrentDictionary<Type, Type>();
        }

        [NotNull]
        public Type ServiceTypeGenericTypeDefinition { get; }

        public Type GetClosedImplTypeOrNull(Type closedServiceType)
        {
            if (this._map.TryGetValue(closedServiceType, out var closedImplType)) return closedImplType;
            var r = this._tree.TryMakeGenericType(closedServiceType, out closedImplType);
            Debug.Assert(r && closedImplType != null || !r && closedImplType == null);
            return this._map.GetOrAdd(closedServiceType, closedImplType);
        }

        public class GenericTypeTree
        {
            [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
            private class Node
            {
                private readonly Type _type;
                private readonly TypeInfo _typeInfo;
                private readonly bool _isGenericType;
                private readonly Node[] _genericArguments;
                private readonly bool _isGenericParameter;

                public Node(GenericTypeTree tree, Type type)
                {
                    this._type = type;
                    this._typeInfo = type.GetTypeInfo();
                    this._isGenericType = this._typeInfo.IsGenericType;
                    if (this._isGenericType)
                    {
                        var args = this._typeInfo.IsGenericTypeDefinition
                            ? this._typeInfo.GenericTypeParameters
                            : this._typeInfo.GenericTypeArguments;
                        this._genericArguments = args.Select(z => new Node(tree, z)).ToArray();
                    }
                    else
                    {
                        this._isGenericParameter = type.IsGenericParameter;
                        if (this._isGenericParameter)
                        {
                            if (!tree._genericParameterConstraints.ContainsKey(type.Name))
                            {
                                tree._genericParameterConstraints[type.Name] =
                                    new GenericParameterConstraint(type, this._typeInfo);
                            }
                        }
                    }
                }

                public bool TryResolveGenericParameter(Type type, Dictionary<string, Type> genericParameterMap)
                {
                    if (this._isGenericType)
                    {
                        var typeInfo = type.GetTypeInfo();
                        if (!typeInfo.IsGenericType)
                        {
                            return false;
                        }
                        var args = typeInfo.GenericTypeArguments;
                        if (args.Length != this._genericArguments.Length)
                        {
                            return false;
                        }
                        for (var i = 0; i < args.Length; i++)
                        {
                            if (!this._genericArguments[i].TryResolveGenericParameter(args[i], genericParameterMap))
                            {
                                return false;
                            }
                        }
                    }
                    else if (this._isGenericParameter)
                    {
                        if (genericParameterMap.TryGetValue(this._type.Name, out var t))
                        {
                            if (t != type) return false;
                        }
                        else
                        {
                            genericParameterMap.Add(this._type.Name, type);
                        }
                    }
                    else
                    {
                        if (type != this._type) return false;
                    }

                    return true;
                }

                public string DebuggerDisplay()
                {
                    if (this._isGenericType)
                    {
                        var name = this._type.Name;
                        var index = name.IndexOf('`');
                        name = name.Substring(0, index);
                        return $"{name}<{string.Join(", ", this._genericArguments.Select(z => z.DebuggerDisplay()).ToArray())}>";
                    }
                    else
                    {
                        return this._type.Name;
                    }
                }
            }

            private class GenericParameterConstraint
            {
                private readonly Type _type;
                private readonly TypeInfo _typeInfo;
                private readonly GenericParameterAttributes _genericParameterAttributes;
                private readonly TypeInfo[] _genericParameterConstraints;

                public GenericParameterConstraint(Type type, TypeInfo typeInfo)
                {
                    this._type = type;
                    this._typeInfo = typeInfo;
                    this._genericParameterAttributes = typeInfo.GenericParameterAttributes;
                    this._genericParameterConstraints = typeInfo
                        .GetGenericParameterConstraints()
                        .Select(z => z.GetTypeInfo())
                        .ToArray();
                }

                public bool MatchConstraints(Dictionary<string, Type> genericParameterMap)
                {
                    if (!genericParameterMap.TryGetValue(this._type.Name, out var type))
                    {
                        return false;
                    }

                    if (this.IsValueType || this.IsClass || this.HasDefaultConstructor ||
                        this._genericParameterConstraints.Length > 0)
                    {
                        var typeInfo = type.GetTypeInfo();

                        if (this.IsValueType && !typeInfo.IsValueType)
                        {
                            return false;
                        }

                        if (this.IsClass && typeInfo.IsValueType)
                        {
                            return false;
                        }

                        if (this.HasDefaultConstructor &&
                            !typeInfo.IsValueType &&
                            typeInfo.DeclaredConstructors.FirstOrDefault(z => z.GetParameters().Length == 0) == null)
                        {
                            return false;
                        }

                        var result = this._genericParameterConstraints.Length == 0 ||
                                     this._genericParameterConstraints.All(z => z.IsAssignableFrom(typeInfo));
                        return result;
                    }
                    else
                    {
                        return true;
                    }
                }

                private bool IsValueType => GenericParameterAttributes.NotNullableValueTypeConstraint
                                            == (this._genericParameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint);

                private bool IsClass => GenericParameterAttributes.ReferenceTypeConstraint
                                        == (this._genericParameterAttributes & GenericParameterAttributes.ReferenceTypeConstraint);

                private bool HasDefaultConstructor => GenericParameterAttributes.DefaultConstructorConstraint
                                                      == (this._genericParameterAttributes & GenericParameterAttributes.DefaultConstructorConstraint);
            }

            private readonly Dictionary<string, GenericParameterConstraint> _genericParameterConstraints
                = new Dictionary<string, GenericParameterConstraint>();
            private readonly Node _serviceNode;
            private readonly Type _implementationType;
            private readonly TypeInfo _implementationTypeInfo;
            private readonly string[] _implementationTypeGenericParameterNames;

            public GenericTypeTree(Type serviceType, Type implementationType)
            {
                this._serviceNode = new Node(this, serviceType);
                this._implementationType = implementationType;
                this._implementationTypeInfo = implementationType.GetTypeInfo();
                if (!this._implementationTypeInfo.IsGenericTypeDefinition)
                {
                    throw new ArgumentException($"implementation type {this._implementationTypeInfo} should be generic type definition.");
                }
                this._implementationTypeGenericParameterNames = this._implementationTypeInfo
                    .GenericTypeParameters
                    .Select(z => z.Name)
                    .ToArray();
                if (this._implementationTypeGenericParameterNames.Length != this._genericParameterConstraints.Count)
                {
                    throw new ArgumentException("generic parameter count is not match.");
                }
                if (this._implementationTypeGenericParameterNames.Any(z => !this._genericParameterConstraints.ContainsKey(z)))
                {
                    throw new ArgumentException("generic parameter is not match.");
                }
            }

            public bool TryMakeGenericType(Type closedServiceType, out Type closedImplType)
            {
                var destTypeInfo = closedServiceType.GetTypeInfo();
                if (destTypeInfo.ContainsGenericParameters)
                {
                    closedImplType = null;
                    return false;
                }
                var map = new Dictionary<string, Type>();
                if (!this._serviceNode.TryResolveGenericParameter(closedServiceType, map))
                {
                    closedImplType = null;
                    return false;
                }
                foreach (var kvp in this._genericParameterConstraints)
                {
                    if (!kvp.Value.MatchConstraints(map))
                    {
                        closedImplType = null;
                        return false;
                    }
                }
                var types = this._implementationTypeGenericParameterNames.Select(z => map[z]).ToArray();
                closedImplType = this._implementationType.MakeGenericType(types);
                return true;
            }
        }
    }

    internal class ComplexGenericsResolver<T> : ComplexGenericsResolver
    {
        public ComplexGenericsResolver([NotNull] Type serviceType, [NotNull] Type implementationType)
            : base(serviceType, implementationType)
        {
        }
    }
}
