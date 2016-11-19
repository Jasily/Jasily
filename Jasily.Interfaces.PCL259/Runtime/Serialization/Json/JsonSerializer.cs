using System;
using System.IO;
using System.Runtime.Serialization.Json;
using Jasily.Interfaces.Data;
using JetBrains.Annotations;

namespace Jasily.Interfaces.Runtime.Serialization.Json
{
    public class JsonSerializer : IJsonSerializer
    {
        private readonly DataContractJsonSerializer serializer;

        public JsonSerializer([NotNull] DataContractJsonSerializer serializer)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            this.serializer = serializer;
        }

        public object Deserialize(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            try
            {
                return this.serializer.ReadObject(stream);
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
                this.serializer.WriteObject(stream, obj);
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