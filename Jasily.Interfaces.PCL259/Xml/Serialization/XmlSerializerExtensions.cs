using System;
using System.IO;
using System.Text;
using Jasily.Interfaces.Data;
using JetBrains.Annotations;

namespace Jasily.Interfaces.Xml.Serialization
{
    public static class XmlSerializerExtensions
    {
        public static IXmlSerializerProvider XmlSerializerProvider { get; set; }
            = new XmlSerializerProvider();

        private static IXmlSerializerProvider GetSerializerProvider()
        {
            var provider = XmlSerializerProvider;
            if (provider == null) throw new InvalidOperationException();
            return provider;
        }

        public static T XmlToObject<T>(this Stream stream) => GetSerializerProvider().DeserializeToObject<T>(stream);

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

        public static byte[] ObjectToXml(this object obj) => GetSerializerProvider().SerializeToBytes(obj);
    }
}