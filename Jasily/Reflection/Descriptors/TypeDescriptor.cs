using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jasily.Extensions.System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Reflection.Descriptors
{
    public sealed class TypeDescriptor<T> : Descriptor<Type>
    {
        private TypeInfoDescriptor<T> typeInfo;
        private IReadOnlyList<MethodInfoDescriptor> runtimeMethods;
        private Dictionary<string, IReadOnlyList<MethodInfoDescriptor>> runtimeMethodsMap;
        private IReadOnlyList<PropertyInfoDescriptor> runtimeProperties;
        private Dictionary<string, PropertyInfoDescriptor> runtimePropertiesMap;
        private IReadOnlyList<FieldInfoDescriptor> runtimeFields;
        private Dictionary<string, FieldInfoDescriptor> runtimeFieldsMap;

        public TypeDescriptor()
            : base(typeof(T))
        {
        }

        public TypeInfoDescriptor<T> TypeInfo => this.typeInfo ?? (this.typeInfo = new TypeInfoDescriptor<T>());

        [NotNull]
        [ItemNotNull]
        public IEnumerable<MethodInfoDescriptor> RuntimeMethods()
        {
            return this.runtimeMethods ?? (this.runtimeMethods =
                this.DescriptedObject
                    .GetRuntimeMethods()
                    .Select(z => new MethodInfoDescriptor(z))
                    .ToArray().AsReadOnly());
        }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<MethodInfoDescriptor> RuntimeMethods([NotNull] string methodName)
        {
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));

            var map = this.runtimeMethodsMap ?? (this.runtimeMethodsMap = this.RuntimeMethods()
                .GroupBy(z => z.DescriptedObject.Name)
                .ToDictionary(z => z.Key, z => z.ToArray().AsReadOnly()));
            var methods = map.GetValueOrDefault(methodName);
            return methods?.AsReadOnly() ?? Enumerable.Empty<MethodInfoDescriptor>();
        }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<PropertyInfoDescriptor> RuntimeProperties()
        {
            return this.runtimeProperties ?? (this.runtimeProperties =
                this.DescriptedObject
                    .GetRuntimeProperties()
                    .Select(z => new PropertyInfoDescriptor(z))
                    .ToArray().AsReadOnly());
        }

        [CanBeNull]
        public PropertyInfoDescriptor RuntimeProperty([NotNull] string propertyName)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            var map = this.runtimePropertiesMap ?? (this.runtimePropertiesMap = this.RuntimeProperties()
                .ToDictionary(z => z.DescriptedObject.Name));
            return map.GetValueOrDefault(propertyName);
        }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<FieldInfoDescriptor> RuntimeFields()
        {
            return this.runtimeFields ?? (this.runtimeFields =
                this.DescriptedObject
                    .GetRuntimeFields()
                    .Select(z => new FieldInfoDescriptor(z))
                    .ToArray().AsReadOnly());
        }

        [CanBeNull]
        public FieldInfoDescriptor RuntimeField([NotNull] string fieldName)
        {
            if (fieldName == null) throw new ArgumentNullException(nameof(fieldName));
            var map = this.runtimeFieldsMap ?? (this.runtimeFieldsMap = this.RuntimeFields()
                .ToDictionary(z => z.DescriptedObject.Name));
            return map.GetValueOrDefault(fieldName);
        }
    }
}