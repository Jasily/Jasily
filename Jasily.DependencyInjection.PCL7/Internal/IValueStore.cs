using System;

namespace Jasily.DependencyInjection.Internal
{
    /// <summary>
    /// Here are the positions how to store:
    /// Singleton: store in <see cref="ServiceBuilder"/>;
    /// Scoped: store in <see cref="ServiceProvider"/>;
    /// Transient: NOT Impl.
    /// </summary>
    internal interface IValueStore : IDisposable
    {
        object GetValue(Service service, ServiceProvider provider, Func<ServiceProvider, object> creator);

        object GetValue(ServiceBuilder builder, ServiceProvider provider, Func<ServiceProvider, object> creator);
    }
}