using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace System.Collections.Generic
{
    /// <summary>
    /// <remarks>by IDictionary impl, key may be allow null.</remarks>
    /// </summary>
    public static class DictionaryExtensions
    {
        #region get value or default

        public static TValue GetValueOrDefault<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, TKey key,
            TValue defaultValue = default(TValue))
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            TValue value;
            return dict.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, TKey key,
            [NotNull] Func<TValue> defaultValueFactory)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (defaultValueFactory == null) throw new ArgumentNullException(nameof(defaultValueFactory));

            TValue value;
            return dict.TryGetValue(key, out value) ? value : defaultValueFactory();
        }

        public static TResult GetValueOrDefault<TKey, TValue, TResult>([NotNull] this IDictionary<TKey, TValue> dict,
            TKey key, [NotNull] Func<TValue, TResult> valueSelector, TResult defaultValue = default(TResult))
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (valueSelector == null) throw new ArgumentNullException(nameof(valueSelector));

            TValue value;
            return dict.TryGetValue(key, out value) ? valueSelector(value) : defaultValue;
        }

        public static TResult GetValueOrDefault<TKey, TValue, TResult>([NotNull] this IDictionary<TKey, TValue> dict,
            TKey key, [NotNull] Func<TValue, TResult> valueSelector, [NotNull] Func<TResult> defaultValueFactory)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (valueSelector == null) throw new ArgumentNullException(nameof(valueSelector));
            if (defaultValueFactory == null) throw new ArgumentNullException(nameof(defaultValueFactory));

            TValue value;
            return dict.TryGetValue(key, out value) ? valueSelector(value) : defaultValueFactory();
        }

        public static TValue? GetValueOrNull<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, TKey key)
            where TValue : struct
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            TValue value;
            return dict.TryGetValue(key, out value) ? (TValue?) value : null;
        }

        #endregion

        #region get and / or set value

        /// <summary>
        /// return old value and set to new value.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <returns></returns>
        public static TValue GetValueAndSet<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, TKey key, TValue newValue)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            var ret = dict[key];
            dict[key] = newValue;
            return ret;
        }

        public static TValue GetValueOrAdd<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, [NotNull] TKey key, TValue value)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            TValue ret;
            if (!dict.TryGetValue(key, out ret)) dict.Add(key, ret = value);
            return ret;
        }

        public static TValue GetValueOrAdd<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, TKey key,
            [NotNull] Func<TValue> valueFactory)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (valueFactory == null) throw new ArgumentNullException(nameof(valueFactory));

            TValue ret;
            if (!dict.TryGetValue(key, out ret)) dict.Add(key, ret = valueFactory());
            return ret;
        }

        /// <summary>
        /// use <code>new TValue()</code> to create new instance.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetValueOrAddNewInstance<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, TKey key) where TValue : new()
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            
            TValue ret;
            if (!dict.TryGetValue(key, out ret)) dict.Add(key, ret = new TValue());
            return ret;
        }

        #endregion

        public static bool TryAdd<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (dict.ContainsKey(key)) return false;
            dict.Add(key, value);
            return true;
        }

        public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            return new ReadOnlyDictionary<TKey, TValue>(dict);
        }
    }
}