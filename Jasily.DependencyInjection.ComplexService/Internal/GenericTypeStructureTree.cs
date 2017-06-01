using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Jasily.DependencyInjection.ComplexService.Internal
{
    internal class GenericTypeStructureTree
    {
        [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
        private abstract class Node
        {
            public Type ServiceType { get; }

            protected Node(Type serviceType)
            {
                this.ServiceType = serviceType;
            }

            public abstract bool TryResolveGenericParameter(Type type, Dictionary<string, Type> genericParameterMap);

            public virtual string DebuggerDisplay()
            {
                return this.ServiceType.Name;
            }

            public static Node CreateNode(GenericTypeStructureTree structureTree, Type serviceType)
            {
                var typeInfo = serviceType.GetTypeInfo();

                if (typeInfo.IsGenericType)
                {
                    var args = typeInfo.IsGenericTypeDefinition
                        ? typeInfo.GenericTypeParameters
                        : typeInfo.GenericTypeArguments;

                    return new GenericTypeNode(serviceType, args.Select(z => CreateNode(structureTree, z)).ToArray());
                }
                else if (serviceType.IsGenericParameter)
                {
                    if (!structureTree._genericParameterConstraints.ContainsKey(serviceType.Name))
                    {
                        structureTree._genericParameterConstraints[serviceType.Name] =
                            new GenericParameterConstraint(serviceType, typeInfo);
                    }

                    return new GenericParameterNode(serviceType);
                }
                else
                {
                    return new TypeNode(serviceType);
                }
            }
        }

        private class TypeNode : Node
        {
            public TypeNode(Type serviceType) : base(serviceType)
            {
            }

            public override bool TryResolveGenericParameter(Type type, Dictionary<string, Type> genericParameterMap)
            {
                return type == this.ServiceType;
            }
        }

        private class GenericTypeNode : Node
        {
            private readonly Node[] _genericArguments;

            public GenericTypeNode(Type serviceType, Node[] genericArguments) : base(serviceType)
            {
                this._genericArguments = genericArguments;
            }

            public override bool TryResolveGenericParameter(Type type, Dictionary<string, Type> genericParameterMap)
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

                return true;
            }

            public override string DebuggerDisplay()
            {
                var name = this.ServiceType.Name;
                var index = name.IndexOf('`');
                name = name.Substring(0, index);
                return $"{name}<{string.Join(", ", this._genericArguments.Select(z => z.DebuggerDisplay()).ToArray())}>";
            }
        }

        private class GenericParameterNode : Node
        {
            public GenericParameterNode(Type serviceType) : base(serviceType)
            {
            }

            public override bool TryResolveGenericParameter(Type type, Dictionary<string, Type> genericParameterMap)
            {
                if (genericParameterMap.TryGetValue(this.ServiceType.Name, out var exists))
                {
                    return exists == type;
                }
                else
                {
                    genericParameterMap.Add(this.ServiceType.Name, type);
                    return true;
                }
            }
        }

        private class GenericParameterConstraint
        {
            private readonly Type _type;
            private readonly GenericParameterAttributes _genericParameterAttributes;
            private readonly TypeInfo[] _genericParameterConstraints;

            public GenericParameterConstraint(Type type, TypeInfo typeInfo)
            {
                this._type = type;
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
        private readonly string[] _implementationTypeGenericParameterNames;

        public GenericTypeStructureTree(Type serviceType, Type implementationType)
        {
            this._serviceNode = Node.CreateNode(this, serviceType);
            this._implementationType = implementationType;
            var implementationTypeInfo = implementationType.GetTypeInfo();
            if (!implementationTypeInfo.IsGenericTypeDefinition)
            {
                throw new ArgumentException($"implementation serviceTypeSource {implementationTypeInfo} should be generic serviceTypeSource definition.");
            }
            this._implementationTypeGenericParameterNames = implementationTypeInfo.GenericTypeParameters
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
