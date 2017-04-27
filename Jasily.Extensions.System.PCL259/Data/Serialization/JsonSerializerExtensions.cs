using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Jasily.Core.Data.Serialization;
using JetBrains.Annotations;

namespace System.Data.Serialization
{
    public static class JsonSerializerExtensions
    {
        public static event Func<Type, ISerializer> SerializerFactory;

        static JsonSerializerExtensions()
        {
            SerializerFactory += z => new Serializer(z);
        }

        private class Serializer : ISerializer
        {
            private readonly DataContractJsonSerializer serializer;

            public Serializer(Type type)
            {
                this.serializer = new DataContractJsonSerializer(type);
            }

            public object Deserialize([NotNull] Stream stream, [CanBeNull] Type type)
            {
                if (stream == null) throw new ArgumentNullException(nameof(stream));
                return this.serializer.ReadObject(stream);
            }

            public void Serialize([NotNull] Stream stream, [NotNull] object obj)
            {
                if (stream == null) throw new ArgumentNullException(nameof(stream));
                if (obj == null) throw new ArgumentNullException(nameof(obj));
                this.serializer.WriteObject(stream, obj);
            }
        }

        static ISerializer GetSerializer(Type type)
        {
            var list = SerializerFactory.GetInvocationList();
            var factory = (Func<Type, ISerializer>)list[list.Length - 1];
            return factory(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="SerializationException"></exception>
        /// <returns></returns>
        public static T JsonToObject<T>([NotNull] this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            try
            {
                return (T)GetSerializer(typeof(T)).Deserialize(stream, typeof(T));
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="SerializationException"></exception>
        /// <returns></returns>
        public static T JsonToObject<T>([NotNull] this byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            using (var ms = new MemoryStream(bytes))
            {
                return ms.JsonToObject<T>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="encoding"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="SerializationException"></exception>
        /// <returns></returns>
        public static T JsonToObject<T>([NotNull] this string s, [NotNull] Encoding encoding)
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            return encoding.GetBytes(s).JsonToObject<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="SerializationException"></exception>
        /// <returns></returns>
        public static T JsonToObject<T>([NotNull] this string s) => JsonToObject<T>(s, Encoding.UTF8);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="SerializationException"></exception>
        /// <returns></returns>
        public static byte[] ObjectToJson([NotNull]  this object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            using (var ms = new MemoryStream())
            {
                var serializer = GetSerializer(obj.GetType());

                try
                {
                    serializer.Serialize(ms, obj);
                }
                catch (IOException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new SerializationException(e.Message, e);
                }

                return ms.ToArray();
            }
        }
    }
}
