using System;

namespace Jasily.DependencyInjection.Internal
{
    internal interface IValueStore
    {
        object GetValue(Service service, ServiceProvider provider, Func<ServiceProvider, object> creator);
    }
}