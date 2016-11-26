using System;
using System.Text;
using Jasily.Interfaces;
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
            segment.ThrowIfInvalid(nameof(segment));
            builder.Append(segment.String, segment.StartIndex, segment.Count);
            return builder;
        }
    }
}