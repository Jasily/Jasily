using JetBrains.Annotations;

namespace System.Text
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static StringBuilder Append([NotNull] this StringBuilder builder, StringSegment segment)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            segment.ThrowIfInvalid();
            builder.Append(segment.String, segment.StartIndex, segment.Count);
            return builder;
        }
    }
}