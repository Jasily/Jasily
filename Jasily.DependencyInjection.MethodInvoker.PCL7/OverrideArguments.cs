using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public struct OverrideArguments
    {
        private bool isRecordRequest;
        private List<ParameterInfo> requests;
        private Dictionary<string, object> data;

        public OverrideArguments(bool recordRequest)
        {
            this.data = null;
            this.requests = null;

            this.isRecordRequest = recordRequest;
        }

        public IEnumerable<ParameterInfo> Requests()
        {
            return this.requests?.ToArray() ?? Enumerable.Empty<ParameterInfo>();
        }

        public void AddArgument(string key, object value)
        {
            if (this.data == null) this.data = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            this.data.Add(key, value);
        }

        public bool TryGetValue<T>(ParameterInfo parameter, out T value)
        {
            if (this.isRecordRequest)
            {
                if (this.requests == null) this.requests = new List<ParameterInfo>();
                this.requests.Add(parameter);
            }

            if (this.data != null)
            {
                var result = this.data.TryGetValue(parameter.Name, out var ret);
                if (result)
                {
                    if (ret is T)
                    {
                        value = (T)ret;
                        return true;
                    }
                    throw new InvalidOperationException();
                }
            }
            
            value = default(T);
            return false;
        }
    }
}
