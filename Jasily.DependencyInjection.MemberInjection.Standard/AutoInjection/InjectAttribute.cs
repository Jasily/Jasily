using System;
using System.Collections.Generic;
using System.Text;

namespace Jasily.DependencyInjection.MemberInjection.AutoInjection
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class InjectAttribute : Attribute
    {
        public InjectAttribute(bool isRequired = true)
        {
            this.IsRequired = isRequired;
        }

        public bool IsRequired { get; }
    }
}
