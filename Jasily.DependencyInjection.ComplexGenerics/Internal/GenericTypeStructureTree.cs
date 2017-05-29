using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Jasily.DependencyInjection.ComplexGenerics.Internal
{
    internal class GenericTypeStructureTree
    {
        [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
        private class Node
        {
            private readonly Type _serviceType;
            private readonly TypeInfo _typeInfo;
            private readonly bool _isGenericType;
            private readonly Node[] _genericArguments;
            private readonly bool _isGenericParameter;

            public Node(GenericTypeStructureTree structureTree, Type serviceType)
            {
                this._serviceType = serviceType;
                this._typeInfo = serviceType.GetTypeInfo();
                this._isGenericType = this._typeInfo.IsGenericType;
                if (this._isGenericType)
                {
                    var args = this._typeInfo.IsGenericTypeDefinition
                        ? this._typeInfo.GenericTypeParameters
                        : this._typeInfo.GenericTypeArguments;
                    this._genericArguments = args.Select(z => new Node(structureTree, z)).ToArray();
                }
                else
                {
                    this._isGenericParameter = serviceType.IsGenericParameter;
                    if (this._isGenericParameter)
                    {
                        if (!structureTree._genericParameterConstraints.ContainsKey(serviceType.Name))
                        {
                            structureTree._genericParameterConstraints[serviceType.Name] =
                                new GenericParameterConstraint(serviceType, this._typeInfo);
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
                    if (genericParameterMap.TryGetValue(this._serviceType.Name, out var t))
                    {
                        if (t != type) return false;
                    }
                    else
                    {
                        genericParameterMap.Add(this._serviceType.Name, type);
                    }
                }
                else
                {
                    if (type != this._serviceType) return false;
                }

                return true;
            }

            public string DebuggerDisplay()
            {
                if (this._isGenericType)
                {
                    var name = this._serviceType.Name;
                    var index = name.IndexOf('`');
                    name = name.Substring(0, index);
                    return $"{name}<{string.Join(", ", this._genericArguments.Select(z => z.DebuggerDisplay()).ToArray())}>";
                }
                else
                {
                    return this._serviceType.Name;
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

        public GenericTypeStructureTree(Type serviceType, Type implementationType)
        {
            this._serviceNode = new Node(this, serviceType);
            this._implementationType = implementationType;
            this._implementationTypeInfo = implementationType.GetTypeInfo();
            if (!this._implementationTypeInfo.IsGenericTypeDefinition)
            {
                throw new ArgumentException($"implementation serviceTypeSource {this._implementationTypeInfo} should be generic serviceTypeSource definition.");
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

        public bool TryMakeClosedImplementationType(Type closedServiceType, out Type closedImplType)
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
