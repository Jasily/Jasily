using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.Cache.Descriptors
{
    public sealed class TypeDescriptor<T> : Descriptor<Type>
    {
        private TypeInfoDescriptor<T> typeInfo;
        private Descriptor<MethodInfo>[] runtimeMethods;
        private Dictionary<string, Descriptor<MethodInfo>[]> runtimeMethodsMap;
        private Descriptor<PropertyInfo>[] runtimeProperties;
        private Dictionary<string, Descriptor<PropertyInfo>> runtimePropertiesMap;
        private Descriptor<FieldInfo>[] runtimeFields;
        private Dictionary<string, Descriptor<FieldInfo>> runtimeFieldsMap;

        public TypeDescriptor()
            : base(typeof(T))
        {
        }

        public TypeInfoDescriptor<T> TypeInfo => this.typeInfo ?? (this.typeInfo = new TypeInfoDescriptor<T>());

        [NotNull]
        [ItemNotNull]
        private IEnumerable<Descriptor<MethodInfo>> InternalRuntimeMethods()
        {
            return this.runtimeMethods ?? (this.runtimeMethods =
                this.DescriptedObject
                    .GetRuntimeMethods()
                    .Select(z => new Descriptor<MethodInfo>(z))
                    .ToArray());
        }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<IDescriptor<MethodInfo>> RuntimeMethods()
            => this.InternalRuntimeMethods().AsReadOnly();

        [NotNull]
        [ItemNotNull]
        public IEnumerable<IDescriptor<MethodInfo>> RuntimeMethods([NotNull] string methodName)
        {
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));

            var map = this.runtimeMethodsMap ?? (this.runtimeMethodsMap = this.InternalRuntimeMethods()
                .GroupBy(z => z.DescriptedObject.Name)
                .ToDictionary(z => z.Key, z => z.ToArray()));
            var methods = map.GetValueOrDefault(methodName);
            return methods?.AsReadOnly() ?? Enumerable.Empty<IDescriptor<MethodInfo>>();
        }

        [NotNull]
        [ItemNotNull]
        private IEnumerable<Descriptor<PropertyInfo>> InternalRuntimeProperties()
        {
            return this.runtimeProperties ?? (this.runtimeProperties =
                this.DescriptedObject
                    .GetRuntimeProperties()
                    .Select(z => new Descriptor<PropertyInfo>(z))
                    .ToArray());
        }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<IDescriptor<PropertyInfo>> RuntimeProperties()
            => this.InternalRuntimeProperties().AsReadOnly();

        [CanBeNull]
        public IDescriptor<PropertyInfo> RuntimeProperty([NotNull] string propertyName)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            var map = this.runtimePropertiesMap ?? (this.runtimePropertiesMap = this.InternalRuntimeProperties()
                .ToDictionary(z => z.DescriptedObject.Name));
            return map.GetValueOrDefault(propertyName);
        }

        [NotNull]
        [ItemNotNull]
        private IEnumerable<Descriptor<FieldInfo>> InternalRuntimeFields()
        {
            return this.runtimeFields ?? (this.runtimeFields =
                this.DescriptedObject
                    .GetRuntimeFields()
                    .Select(z => new Descriptor<FieldInfo>(z))
                    .ToArray());
        }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<IDescriptor<FieldInfo>> RuntimeFields()
            => this.InternalRuntimeFields().AsReadOnly();

        [CanBeNull]
        public IDescriptor<FieldInfo> RuntimeField([NotNull] string fieldName)
        {
            if (fieldName == null) throw new ArgumentNullException(nameof(fieldName));
            var map = this.runtimeFieldsMap ?? (this.runtimeFieldsMap = this.InternalRuntimeFields()
                .ToDictionary(z => z.DescriptedObject.Name));
            return map.GetValueOrDefault(fieldName);
        }
    }
}