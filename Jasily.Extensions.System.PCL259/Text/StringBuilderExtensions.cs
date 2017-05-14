using System;
using System.Text;
using JetBrains.Annotations;

namespace Jasily.Extensions.System.Text
{
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static StringBuilder Append([NotNull] this StringBuilder builder, [NotNull] string value, int startIndex)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (startIndex < 0 || startIndex > value.Length) throw new ArgumentOutOfRangeException(nameof(startIndex));
            builder.Append(value, startIndex, value.Length - startIndex);
            return builder;
        }
    }
}