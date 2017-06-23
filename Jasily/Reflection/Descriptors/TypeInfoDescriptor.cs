using System.Reflection;
using Jasily.Reflection.Descriptors.Internal;

// ReSharper disable StaticMemberInGenericType

namespace Jasily.Reflection.Descriptors
{
    public class TypeInfoDescriptor<T> : MemberInfoDescriptor<TypeInfo>
    {
        public TypeInfoDescriptor()
            : base(typeof(T).GetTypeInfo())
        {
        }
    }
}