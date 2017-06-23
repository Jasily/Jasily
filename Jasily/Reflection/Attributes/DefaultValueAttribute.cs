using System;

namespace Jasily.Reflection.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class DefaultValueAttribute : Attribute
    {
        public DefaultValueAttribute(object value)
        {
            this.Value = value;
        }

        public object Value { get; }
    }
}