using System;

namespace Jasily.Interfaces
{
    public static class InitializedValueTypeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="paramName"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowIfInvalid<T>(this T value, string paramName) where T : struct, IInitializedValueType
        {
            if (!value.IsInitialized)
            {
                throw new ArgumentException("value is invalid", paramName ?? nameof(value));
            }
        }
    }
}