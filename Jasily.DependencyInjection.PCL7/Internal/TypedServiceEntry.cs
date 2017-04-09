using System;
using System.Collections.Generic;

namespace Jasily.DependencyInjection.Internal
{
    internal class TypedServiceEntry : ServiceEntry
    {
        private readonly Dictionary<string, List<Service>> entriesMap = new Dictionary<string, List<Service>>();

        public TypedServiceEntry(Type type, StringComparer comparer)
        {
            this.entriesMap = new Dictionary<string, List<Service>>(comparer);
            this.ServiceType = type;
        }

        public Type ServiceType { get; }

        public Service Last { get; private set; }

        public override void Add(Service service)
        {
            if (!this.entriesMap.TryGetValue(service.ServiceName, out var list))
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
                    if (resolveRequest.ServiceName == string.Empty)
                    {
                        return this.Last;
                    }
                    else
                    {
                        if (this.entriesMap.TryGetValue(resolveRequest.ServiceName, out var list))
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