using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.Reflection.Descriptors
{
    public abstract class Descriptor
    {
        [NotNull, Pure]
        public static TypeInfoDescriptor CreateDescriptor([NotNull] TypeInfo type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return new TypeInfoDescriptor(type);
        }

        [NotNull, Pure]
        public static FieldInfoDescriptor CreateDescriptor([NotNull] FieldInfo field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            return new FieldInfoDescriptor(field);
        }

        [NotNull, Pure]
        public static PropertyInfoDescriptor CreateDescriptor([NotNull] PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            return new PropertyInfoDescriptor(property);
        }

        [NotNull, Pure]
        public static MethodInfoDescriptor CreateDescriptor([NotNull] MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            return new MethodInfoDescriptor(method);
        }
    }

    public abstract class Descriptor<T> : Descriptor
        where T : class
    {
        internal Descriptor([NotNull] T obj)
        {
            this.DescriptedObject = obj ?? throw new ArgumentNullException(nameof(obj));
        }

        public T DescriptedObject { get; }
    }
}