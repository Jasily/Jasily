using System.Reflection;
using Jasily.Cache.Descriptors.Internal;

// ReSharper disable StaticMemberInGenericType

namespace Jasily.Cache.Descriptors
{
    public class TypeInfoDescriptor<T> : MemberInfoDescriptor<TypeInfo>
    {
        public TypeInfoDescriptor()
            : base(typeof(T).GetTypeInfo())
        {
        }
    }
}