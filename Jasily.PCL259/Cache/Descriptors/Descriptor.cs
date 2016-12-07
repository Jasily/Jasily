using System;
using JetBrains.Annotations;

namespace Jasily.Cache.Descriptors
{
    public class Descriptor<T> : IDescriptor<T>
        where T : class 
    {
        public Descriptor([NotNull] T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            this.DescriptedObject = obj;
        }

        public T DescriptedObject { get; }
    }
}