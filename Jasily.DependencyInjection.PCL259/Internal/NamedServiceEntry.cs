using System;
using System.Collections.Generic;

namespace Jasily.DependencyInjection.Internal
{
    internal class NamedServiceEntry : ServiceEntry
    {
        private readonly List<Service> entries = new List<Service>();

        public override void Add(Service service)
        {
            this.entries.Add(service);
        }

        public override Service Resolve(Type serviceType, string serviceName, ResolveLevel level)
        {
            return null;
            throw new NotImplementedException();
        }
    }
}