using System;
using System.IO;
using System.Text;
using Jasily.Interfaces.Data;
using JetBrains.Annotations;

namespace Jasily.Interfaces.Xml.Serialization
{
    public static class XmlSerializerExtensions
    {
        static XmlSerializerExtensions()
        {
            if (JasilySettings<IXmlSerializerProvider>.Value == null)
            {
                JasilySettings<IXmlSerializerProvider>.Value = new XmlSerializerProvider();
            }
        }

        public static T XmlToObject<T>(this Stream stream)
            => JasilySettings<IXmlSerializerProvider>.GetOrThrow().DeserializeToObject<T>(stream);

        public static T XmlToObject<T>([NotNull] this byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            using (var ms = new MemoryStream(bytes))
            {
                return ms.XmlToObject<T>();
            }
        }

        public static T XmlToObject<T>([NotNull] this string s, [NotNull] Encoding encoding)
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            return encoding.GetBytes(s).XmlToObject<T>();
        }

        public static T XmlToObject<T>([NotNull] this string s) => XmlToObject<T>(s, Encoding.UTF8);

        public static byte[] ObjectToXml(this object obj)
            => JasilySettings<IXmlSerializerProvider>.GetOrThrow().SerializeToBytes(obj);
    }
}