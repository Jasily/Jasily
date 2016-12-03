using System.Collections.Generic;

namespace Jasily.DependencyInjection.Internal
{
    internal interface IServiceCallSiteProvider
    {
        IServiceCallSite CreateServiceCallSite(ServiceProvider provider, ISet<Service> serviceChain);
    }
}