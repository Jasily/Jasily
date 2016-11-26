using System;
using JetBrains.Annotations;

namespace Jasily.Interfaces
{
    public static class JasilySettings<T> where T : class
    {
        public static T Value { get; set; }

        [NotNull]
        public static T GetOrThrow(string message = null)
        {
            var obj = Value;
            if (obj == null)
            {
                if (message != null) throw new InvalidOperationException(message);
                throw new InvalidOperationException();
            }
            return obj;
        }
    }
}