using System;

namespace Jasily.DependencyInjection.Internal
{
    internal class ValueStore : IValueStore
    {
        private bool isValueCreated;
        private object value;

        public void Dispose()
        {
            this.isValueCreated = false;
            (this.value as IDisposable)?.Dispose();
            this.value = null;
        }

        public object GetValue(Service service, ServiceProvider provider, Func<ServiceProvider, object> creator)
        {
            if (this.isValueCreated) return this.value;

            lock (this)
            {
                if (!this.isValueCreated)
                {
                    this.value = creator(provider);
                    this.isValueCreated = true;
                }
            }

            return this.value;
        }
    }
}