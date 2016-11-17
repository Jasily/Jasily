using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Interfaces.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static void Add<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, [NotNull] TValue item)
            where TValue : IGetKey<TKey>
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (item == null) throw new ArgumentNullException(nameof(item));

            dict.Add(item.GetKey(), item);
        }

        public static void AddRange<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict,
            [NotNull] IEnumerable<TValue> items)
            where TValue : IGetKey<TKey>
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (items == null) throw new ArgumentNullException(nameof(items));

            foreach (var value in items)
            {
                dict.Add(value.GetKey(), value);
            }
        }

        public static void Set<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, [NotNull] TValue item)
            where TValue : IGetKey<TKey>
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (item == null) throw new ArgumentNullException(nameof(item));

            dict[item.GetKey()] = item;
        }
    }
}