using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class Enumerable2
    {
        public static List<T> ToList<T>([NotNull] this IEnumerable<T> source, int capacity)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var list = new List<T>(capacity);
            list.AddRange(source);
            return list;
        }

        public static LinkedList<T> ToLinkedList<T>([NotNull] this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return new LinkedList<T>(source);
        }
    }
}
