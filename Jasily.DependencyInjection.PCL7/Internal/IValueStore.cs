using System;

namespace Jasily.DependencyInjection.Internal
{
    internal interface IValueStore : IDisposable
    {
        object GetValue(Service service, ServiceProvider provider, Func<ServiceProvider, object> creator);
    }
}