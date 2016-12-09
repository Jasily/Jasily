using System.Reflection;
using Jasily.Reflection.Descriptors.Internal;
using JetBrains.Annotations;

namespace Jasily.Reflection.Descriptors
{
    public sealed class FieldInfoDescriptor : MemberInfoDescriptor<FieldInfo>
    {
        internal FieldInfoDescriptor([NotNull] FieldInfo obj)
            : base(obj)
        {
        }
    }
}