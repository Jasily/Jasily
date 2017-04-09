using System;
using System.Threading;
using Jasily.Core;

namespace Jasily.DependencyInjection.Internal
{
    internal class ValueStore : IValueStore
    {
        private ValueContainer<object> value;

        public void Dispose()
        {
            (this.value?.Value as IDisposable)?.Dispose();
            this.value = null;
        }

        public object GetValue(ServiceBuilder builder, ServiceProvider provider, Func<ServiceProvider, object> creator)
        {
            var vc = this.value;
            if (vc == null)
            {
                Interlocked.CompareExchange(ref this.value, new ValueContainer<object>(creator(provider)), null);
                vc = this.value;
            }
            return vc.Value;
        }
    }
}