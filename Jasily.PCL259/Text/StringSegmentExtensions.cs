using System;
using System.Text;
using JetBrains.Annotations;

namespace Jasily.Text
{
    public static class StringSegmentExtensions
    {
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
            segment.EnsureNotNull();
            builder.Append(segment.String, segment.StartIndex, segment.Count);
            return builder;
        }

        public static StringSegment ToSegment([NotNull] this string str) => new StringSegment(str);
    }
}