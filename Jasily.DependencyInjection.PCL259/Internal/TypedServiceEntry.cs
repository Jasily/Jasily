using System.Collections.Generic;

namespace Jasily.DependencyInjection.Internal
{
    internal class TypedServiceEntry : ServiceEntry
    {
        private readonly Dictionary<string, List<Service>> entriesMap = new Dictionary<string, List<Service>>();

        public Service Last { get; private set; }

        public override void Add(Service service)
        {
            List<Service> list;
            if (!this.entriesMap.TryGetValue(service.ServiceName, out list))
            {
                list = new List<Service>(1);
                this.entriesMap.Add(service.ServiceName, list);
            }
            list.Add(service);
            this.Last = service;
        }

        public override Service Resolve(ResolveRequest resolveRequest, ResolveLevel level)
        {
            switch (level)
            {
                case ResolveLevel.TypeAndName:
                    if (resolveRequest.ServiceName == null)
                    {
                        return this.Last;
                    }
                    else
                    {
                        List<Service> list;
                        if (this.entriesMap.TryGetValue(resolveRequest.ServiceName, out list))
                        {
                            return list[list.Count - 1];
                        }
                    }
                    break;

                case ResolveLevel.Type:
                    return this.Last;
            }
            return null;
        }
    }
}