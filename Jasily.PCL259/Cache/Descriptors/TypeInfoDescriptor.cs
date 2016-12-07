using System.Reflection;

// ReSharper disable StaticMemberInGenericType

namespace Jasily.Cache.Descriptors
{
    public class TypeInfoDescriptor<T> : Descriptor<TypeInfo>
    {
        public TypeInfoDescriptor()
            : base(typeof(T).GetTypeInfo())
        {
        }
    }
}