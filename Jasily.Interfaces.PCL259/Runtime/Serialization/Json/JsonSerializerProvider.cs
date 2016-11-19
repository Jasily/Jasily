using System;
using System.Runtime.Serialization.Json;
using Jasily.Interfaces.Data;

namespace Jasily.Interfaces.Runtime.Serialization.Json
{
    public class JsonSerializerProvider : IJsonSerializerProvider
    {
        ISerializer ISerializerProvider.Create(Type type) => this.Create(type);

        public IJsonSerializer Create(Type type) => new JsonSerializer(new DataContractJsonSerializer(type));
    }
}