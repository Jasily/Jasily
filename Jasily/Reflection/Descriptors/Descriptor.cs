using System;
using JetBrains.Annotations;

namespace Jasily.Reflection.Descriptors
{
    public class Descriptor<T> : IDescriptor<T>
        where T : class
    {
        public Descriptor([NotNull] T obj)
        {
            this.DescriptedObject = obj ?? throw new ArgumentNullException(nameof(obj));
        }

        public T DescriptedObject { get; }
    }
}