using System;
using Jasily.Interfaces.Data;

namespace Jasily.Interfaces.Xml.Serialization
{
    public class XmlSerializerProvider : IXmlSerializerProvider
    {
        ISerializer ISerializerProvider.Create(Type type) => this.Create(type);

        public IXmlSerializer Create(Type type) => new XmlSerializer(type);
    }
}