using System;
using Jasily.Interfaces.Data;
using JetBrains.Annotations;

namespace Jasily.Interfaces.Xml.Serialization
{
    public interface IXmlSerializerProvider : ISerializerProvider
    {
        [NotNull]
        new IXmlSerializer Create([NotNull] Type type);
    }
}