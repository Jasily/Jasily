using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Jasily.Extensions.System.Collections.Generic;
using JetBrains.Annotations;

// ReSharper disable StaticMemberInGenericType

namespace Jasily.Reflection.Descriptors
{
    public class TypeInfoDescriptor : MemberInfoDescriptor<TypeInfo>
    {
        [CanBeNull] private IReadOnlyList<MethodInfoDescriptor> _runtimeMethodDescriptors;
        [CanBeNull] private Dictionary<string, IReadOnlyList<MethodInfoDescriptor>> _runtimeMethodsMap;
        [CanBeNull] private IReadOnlyList<PropertyInfoDescriptor> _runtimePropertyDescriptors;
        [CanBeNull] private Dictionary<string, PropertyInfoDescriptor> _runtimePropertyDescriptorsMap;
        [CanBeNull] private IReadOnlyList<FieldInfoDescriptor> _runtimeFieldDescriptors;
        [CanBeNull] private Dictionary<string, FieldInfoDescriptor> _runtimeFieldDescriptorsMap;

        internal TypeInfoDescriptor(TypeInfo obj) : base(obj)
        {
        }

        internal TypeInfoDescriptor(Type obj) : this(obj?.GetTypeInfo())
        {
        }

        [NotNull, ItemNotNull]
        public IReadOnlyList<MethodInfoDescriptor> RuntimeMethodDescriptors
        {
            get
            {
                if (this._runtimeMethodDescriptors == null)
                {
                    Interlocked.CompareExchange(ref this._runtimeMethodDescriptors, this.DescriptedObject
                        .GetRuntimeMethods()
                        .Select(CreateDescriptor)
                        .ToArray()
                        .AsReadOnly(), null);
                }
                return this._runtimeMethodDescriptors;
            }
        }

        [NotNull, ItemNotNull]
        public IEnumerable<MethodInfoDescriptor> GetRuntimeMethodDescriptors([NotNull] string methodName)
        {
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));

            if (this._runtimeMethodsMap == null)
            {
                var map = this.RuntimeMethodDescriptors
                    .GroupBy(z => z.DescriptedObject.Name)
                    .ToDictionary(z => z.Key, z => z.ToArray().AsReadOnly());
                Interlocked.CompareExchange(ref this._runtimeMethodsMap, map, null);
            }

            var methods = this._runtimeMethodsMap.GetValueOrDefault(methodName);
            return methods ?? Enumerable.Empty<MethodInfoDescriptor>();
        }

        [NotNull, ItemNotNull]
        public IReadOnlyList<PropertyInfoDescriptor> RuntimePropertyDescriptors
        {
            get
            {
                if (this._runtimePropertyDescriptors == null)
                {
                    Interlocked.CompareExchange(ref this._runtimePropertyDescriptors, this.DescriptedObject
                        .GetRuntimeProperties()
                        .Select(CreateDescriptor)
                        .ToArray()
                        .AsReadOnly(), null);
                }
                return this._runtimePropertyDescriptors;
            }
        }

        [CanBeNull]
        public PropertyInfoDescriptor RuntimePropertyDescriptor([NotNull] string propertyName)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            if (this._runtimePropertyDescriptorsMap == null)
            {
                Interlocked.CompareExchange(ref this._runtimePropertyDescriptorsMap, this.RuntimePropertyDescriptors
                    .ToDictionary(z => z.DescriptedObject.Name), null);
            }
            return this._runtimePropertyDescriptorsMap.GetValueOrDefault(propertyName);
        }

        [NotNull, ItemNotNull]
        public IReadOnlyList<FieldInfoDescriptor> RuntimeFieldDescriptors
        {
            get
            {
                if (this._runtimeFieldDescriptors == null)
                {
                    Interlocked.CompareExchange(ref this._runtimeFieldDescriptors, this.DescriptedObject
                        .GetRuntimeFields()
                        .Select(CreateDescriptor)
                        .ToArray()
                        .AsReadOnly(), null);
                }
                return this._runtimeFieldDescriptors;
            }
        }

        [CanBeNull]
        public FieldInfoDescriptor RuntimeFieldDescriptor([NotNull] string fieldName)
        {
            if (fieldName == null) throw new ArgumentNullException(nameof(fieldName));
            if (this._runtimeFieldDescriptorsMap == null)
            {
                Interlocked.CompareExchange(ref this._runtimeFieldDescriptorsMap, this.RuntimeFieldDescriptors
                    .ToDictionary(z => z.DescriptedObject.Name), null);
            }
            return this._runtimeFieldDescriptorsMap.GetValueOrDefault(fieldName);
        }
    }
}