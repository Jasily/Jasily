using System.Collections.Generic;
using System.Linq;

namespace Jasily.DependencyInjection.Internal
{
    internal class NamedServiceEntry : ServiceEntry
    {
        private readonly List<Service> entries = new List<Service>();

        public override void Add(Service service) => this.entries.Add(service);

        public override Service Resolve(ResolveRequest resolveRequest, ResolveLevel level)
        {
            var service = this.entries.FirstOrDefault(z => z.ServiceType == resolveRequest.ServiceType);
            if (service != null) return service;
            for (var i = this.entries.Count - 1; i >= 0; i--)
            {
                service = this.entries[i];
                if (resolveRequest.ServiceTypeInfo.IsAssignableFrom(service.ServiceTypeInfo))
                {
                    return service;
                }
            }
            return null;
        }
    }
}