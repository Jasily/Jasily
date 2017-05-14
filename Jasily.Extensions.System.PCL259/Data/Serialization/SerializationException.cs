using System;

namespace Jasily.Extensions.System.Data.Serialization
{
    public class SerializationException : Exception
    {
        public SerializationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
