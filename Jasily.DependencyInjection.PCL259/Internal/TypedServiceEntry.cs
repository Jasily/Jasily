using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Jasily.DependencyInjection.Internal
{
    internal class TypedServiceEntry : ServiceEntry
    {
        private List<Service> entries;
        private Dictionary<string, List<Service>> entriesMap;

        public Service Last { get; private set; }

        public override void Add(Service service)
        {
            if (this.entries == null && this.entriesMap == null) // 0 records.
            {
                this.entries = new List<Service>(1) { service };
            }
            else
            {
                if (this.entries != null) // lessthen 10 records
                {
                    Debug.Assert(this.entriesMap == null);
                    if (this.entries.Count >= 9)
                    {
                        this.entriesMap = new Dictionary<string, List<Service>>(StringComparer);
                        foreach (var entry in this.entries)
                        {
                            this.AddToEntriesMap(entry);
                        }
                        this.entries = null;
                    }
                    else
                    {
                        this.entries.Add(service);
                    }
                }

                if (this.entriesMap != null)
                {
                    Debug.Assert(this.entries == null);
                    this.AddToEntriesMap(service);
                }
            }

            this.Last = service;
        }

        private void AddToEntriesMap(Service service)
        {
            Debug.Assert(this.entriesMap != null);

            List<Service> list;
            if (!this.entriesMap.TryGetValue(service.ServiceName, out list))
            {
                list = new List<Service>(1);
                this.entriesMap.Add(service.ServiceName, list);
            }
            list.Add(service);
        }

        public override Service Resolve(Type serviceType, string serviceName, ResolveLevel level)
        {
            switch (level)
            {
                case ResolveLevel.TypeAndName:
                    if (serviceName == null)
                    {
                        return this.Last;
                    }
                    else if (this.entries != null)
                    {
                        return this.entries.FirstOrDefault(z => StringComparer.Equals(serviceName, z.ServiceName));
                    }
                    else if (this.entriesMap != null)
                    {
                        List<Service> list;
                        if (this.entriesMap.TryGetValue(serviceName, out list))
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