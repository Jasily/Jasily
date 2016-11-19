using System;
using System.IO;
using Jasily.Interfaces.Data;

namespace Jasily.Interfaces.Xml.Serialization
{
    public class XmlSerializer : IXmlSerializer
    {
        private readonly Type type;
        private readonly System.Xml.Serialization.XmlSerializer serializer;

        public XmlSerializer(Type type)
        {
            this.type = type;
            this.serializer = new System.Xml.Serialization.XmlSerializer(type);
        }

        public object Deserialize(Stream stream) => this.serializer.Deserialize(stream);

        public object TryDeserialize(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            try
            {
                return this.Deserialize(stream);
            }
            catch (IOException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new SerializationException(e.Message, e);
            }
        }

        public void Serialize(Stream stream, object obj)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            try
            {
                this.serializer.Serialize(stream, obj);
            }
            catch (IOException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new SerializationException(e.Message, e);
            }
        }
    }
}