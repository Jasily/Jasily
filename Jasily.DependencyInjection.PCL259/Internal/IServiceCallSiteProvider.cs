using System.Collections.Generic;
using Jasily.DependencyInjection.Internal.CallSites;

namespace Jasily.DependencyInjection.Internal
{
    internal interface IServiceCallSiteProvider
    {
        IServiceCallSite CreateServiceCallSite(ServiceProvider provider, ISet<Service> serviceChain);
    }
}