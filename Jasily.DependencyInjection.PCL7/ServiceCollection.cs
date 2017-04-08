using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using ServiceDescriptor = Microsoft.Extensions.DependencyInjection.ServiceDescriptor;

namespace Jasily.DependencyInjection
{
    /// <summary>
    /// Default implementation of <see cref="IServiceCollection"/>.
    /// </summary>
    public class ServiceCollection : IServiceCollection
    {
        private readonly List<ServiceDescriptor> descriptors = new List<ServiceDescriptor>();

        /// <inheritdoc />
        public int Count => this.descriptors.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        public ServiceDescriptor this[int index]
        {
            get => this.descriptors[index];
            set => this.descriptors[index] = value;
        }

        /// <inheritdoc />
        public void Clear() => this.descriptors.Clear();

        /// <inheritdoc />
        public bool Contains(ServiceDescriptor item) => this.descriptors.Contains(item);

        /// <inheritdoc />
        public void CopyTo(ServiceDescriptor[] array, int arrayIndex) => this.descriptors.CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public bool Remove(ServiceDescriptor item) => this.descriptors.Remove(item);

        /// <inheritdoc />
        public IEnumerator<ServiceDescriptor> GetEnumerator() => this.descriptors.GetEnumerator();

        void ICollection<ServiceDescriptor>.Add(ServiceDescriptor item) => this.descriptors.Add(item);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public int IndexOf(ServiceDescriptor item) => this.descriptors.IndexOf(item);

        public void Insert(int index, ServiceDescriptor item) => this.descriptors.Insert(index, item);

        public void RemoveAt(int index) => this.descriptors.RemoveAt(index);
    }
}