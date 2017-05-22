using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.ComplexGenerics.Internal
{
    internal class TypeInfoAccessor
    {
        public TypeInfoAccessor(TypeInfoAccessorFactory factory, Type type)
        {
            this.Type = type;
            this.TypeInfo = type.GetTypeInfo();
            this.IsGenericType = this.TypeInfo.IsGenericType;
            if (this.IsGenericType)
            {
                if (this.TypeInfo.IsGenericTypeDefinition)
                {
                    this.GenericTypeDefinition = this;
                }
                else
                {
                    this.GenericTypeDefinition = factory.GetAccessor(this.TypeInfo.GetGenericTypeDefinition());
                    this.ComplexServiceProviderType = this.GenericTypeDefinition.ComplexServiceProviderType;
                }
            }

            if (this.ComplexServiceProviderType == null)
            {
                this.ComplexServiceProviderType = typeof(ComplexServiceProvider<>).MakeGenericType(type);
            }
        }

        [NotNull]
        public Type Type { get; }

        [NotNull]
        public TypeInfo TypeInfo { get; }

        public bool IsGenericType { get; }

        [CanBeNull]
        public TypeInfoAccessor GenericTypeDefinition { get; }

        [NotNull]
        public Type ComplexServiceProviderType { get; }
    }
}