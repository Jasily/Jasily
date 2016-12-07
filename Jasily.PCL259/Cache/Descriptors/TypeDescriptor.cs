using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.Cache.Descriptors
{
    public sealed class TypeDescriptor<T> : Descriptor<Type>
    {
        private IDescriptor<MethodInfo>[] runtimeMethods;
        private Dictionary<string, IDescriptor<MethodInfo>[]> runtimeMethodsMap;

        public TypeDescriptor()
            : base(typeof(T))
        {
        }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<IDescriptor<MethodInfo>> RuntimeMethods()
        {
            var methods = this.runtimeMethods ?? (this.runtimeMethods =
                this.DescriptedObject
                .GetRuntimeMethods()
                .Select(z => new Descriptor<MethodInfo>(z))
                .ToArray());
            return methods.AsReadOnly();
        }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<IDescriptor<MethodInfo>> RuntimeMethods([NotNull] string methodName)
        {
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));

            var map = this.runtimeMethodsMap ?? (this.runtimeMethodsMap = this.RuntimeMethods()
                .GroupBy(z => z.DescriptedObject.Name)
                .ToDictionary(z => z.Key, z => z.ToArray()));
            var methods = map.GetValueOrDefault(methodName);
            return methods?.AsReadOnly() ?? Enumerable.Empty<IDescriptor<MethodInfo>>();
        }
    }
}