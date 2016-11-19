using System;
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;

namespace Jasily.Interfaces.Data
{
    public static class SerializerProviderExtensions
    {
        public static T DeserializeToObject<T>([NotNull] this ISerializerProvider provider, [NotNull] Stream stream)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (stream == null) throw new ArgumentNullException(nameof(stream));

#if DEBUG
            var bytes = stream.ToArray();
            stream = bytes.ToMemoryStream();
#endif

            try
            {
                return (T)provider.Create(typeof(T)).Deserialize(stream);
            }
            catch (Exception)
            {
#if DEBUG
                Debug.WriteLine("deserialize() failed: {0}", bytes.GetString());
#endif
                throw;
            }
        }

        public static byte[] SerializeToBytes([NotNull] this ISerializerProvider provider, [NotNull] object obj)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            using (var ms = new MemoryStream())
            {
                provider.Create(obj.GetType()).Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}