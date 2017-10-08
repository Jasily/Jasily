using System;
using JetBrains.Annotations;

namespace Jasily
{
    public static class Size
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        [PublicAPI]
        public static int SizeOfPrimitiveType(Type type)
        {
            if (type == typeof(bool)) return sizeof(bool);
            if (type == typeof(byte)) return sizeof(byte);
            if (type == typeof(sbyte)) return sizeof(sbyte);
            if (type == typeof(short)) return sizeof(short);
            if (type == typeof(ushort)) return sizeof(ushort);
            if (type == typeof(int)) return sizeof(int);
            if (type == typeof(uint)) return sizeof(uint);
            if (type == typeof(long)) return sizeof(long);
            if (type == typeof(ulong)) return sizeof(ulong);
            if (type == typeof(char)) return sizeof(char);
            if (type == typeof(double)) return sizeof(double);
            if (type == typeof(float)) return sizeof(float);
            throw new NotSupportedException();
        }
    }
}