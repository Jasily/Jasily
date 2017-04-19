using System;
using System.Collections.Generic;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public struct OverrideArguments : IArguments
    {
        private Dictionary<string, object> data;

        public void AddArgument(string key, object value)
        {
            if (this.data == null) this.data = new Dictionary<string, object>();
            this.data.Add(key, value);
        }

        public bool TryGetValue(string key, out object value)
        {
            if (this.data != null)
            {
                return this.data.TryGetValue(key, out value);
            }
            else
            {
                value = null;
                return false;
            }
        }
    }
}
