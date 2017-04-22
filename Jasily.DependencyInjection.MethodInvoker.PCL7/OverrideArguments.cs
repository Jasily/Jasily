using System;
using System.Collections.Generic;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public struct OverrideArguments
    {
        private Dictionary<string, object> data;

        public void AddArgument(string key, object value)
        {
            if (this.data == null) this.data = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            this.data.Add(key, value);
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            var result = this.data.TryGetValue(key, out var ret);
            if (result)
            {
                if (ret is T)
                {
                    value = (T)ret;
                    return true;
                }
                throw new InvalidOperationException();
            }
            value = default(T);
            return false;
        }
    }
}
