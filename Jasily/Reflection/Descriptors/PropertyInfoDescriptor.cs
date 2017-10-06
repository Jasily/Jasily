using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.Reflection.Descriptors
{
    public sealed class PropertyInfoDescriptor : MemberInfoDescriptor<PropertyInfo>
    {
        internal PropertyInfoDescriptor([NotNull] PropertyInfo obj)
            : base(obj)
        {
        }
    }
}