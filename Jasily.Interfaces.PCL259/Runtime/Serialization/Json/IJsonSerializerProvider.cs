using System;
using Jasily.Interfaces.Data;
using JetBrains.Annotations;

namespace Jasily.Interfaces.Runtime.Serialization.Json
{
    public interface IJsonSerializerProvider : ISerializerProvider
    {
        new IJsonSerializer Create([NotNull] Type type);
    }
}