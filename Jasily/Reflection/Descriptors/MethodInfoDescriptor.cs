using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.Reflection.Descriptors
{
    public sealed class MethodInfoDescriptor : MemberInfoDescriptor<MethodInfo>
    {
        internal MethodInfoDescriptor([NotNull] MethodInfo obj)
            : base(obj)
        {
            
        }
    }
}