using System.Reflection;

// ReSharper disable StaticMemberInGenericType

namespace Jasily.Cache
{
    public static class TypeInfoDescriptor<T>
    {
        public static TypeInfo TypeInfo { get; }

        static TypeInfoDescriptor()
        {
            TypeInfo = typeof(T).GetTypeInfo();
        }
    }
}