using System;
using System.Threading;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Internal
{
    internal class TypeDescriptor : IValueStore
    {
        private int count;
        private readonly ValueStore valueStore = new ValueStore();

        [NotNull]
        public Type ImplementationType { get; }

        public TypeDescriptor([NotNull] Type implementationType, int count = 1)
        {
            this.count = count;
            this.ImplementationType = implementationType;
        }

        public void Dispose()
        {
            if (Interlocked.Decrement(ref this.count) == 0)
            {
                this.valueStore.Dispose();
            }
        }

        public object GetValue(Service service, ServiceProvider provider, Func<ServiceProvider, object> creator)
            => this.valueStore.GetValue(service, provider, creator);
    }
}