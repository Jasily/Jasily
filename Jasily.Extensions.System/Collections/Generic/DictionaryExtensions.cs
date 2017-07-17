using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Jasily.Core;
using JetBrains.Annotations;

namespace Jasily.Extensions.System.Collections.Generic
{
    /// <summary>
    /// <remarks>by IDictionary impl, key may be allow null.</remarks>
    /// </summary>
    public static class DictionaryExtensions
    {
        #region get value or default

        /// <summary>
        /// Get value from <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if <paramref name="dict"/> or <paramref name="key"/> is null.</exception>
        public static TValue GetValueOrDefault<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, [NotNull] TKey key,
            TValue defaultValue = default(TValue))
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            return dict.TryGetValue(key, out TValue value) ? value : defaultValue;
        }

        /// <summary>
        /// Get value from <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultValueFactory"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if one of arguments is null.</exception>
        public static TValue GetValueOrDefault<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, [NotNull] TKey key,
            [NotNull] Func<TValue> defaultValueFactory)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (defaultValueFactory == null) throw new ArgumentNullException(nameof(defaultValueFactory));

            return dict.TryGetValue(key, out TValue value) ? value : defaultValueFactory();
        }

        /// <summary>
        /// Get value from <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="valueSelector"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// throw if <paramref name="dict"/> or <paramref name="key"/> or <paramref name="valueSelector"/> is null.
        /// </exception>
        public static TResult GetValueOrDefault<TKey, TValue, TResult>([NotNull] this IDictionary<TKey, TValue> dict, [NotNull] TKey key,
            [NotNull] Func<TValue, TResult> valueSelector, TResult defaultValue = default(TResult))
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (valueSelector == null) throw new ArgumentNullException(nameof(valueSelector));

            return dict.TryGetValue(key, out TValue value) ? valueSelector(value) : defaultValue;
        }

        /// <summary>
        /// Get value from <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="valueSelector"></param>
        /// <param name="defaultValueFactory"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if one of arguments is null.</exception>
        public static TResult GetValueOrDefault<TKey, TValue, TResult>([NotNull] this IDictionary<TKey, TValue> dict, [NotNull] TKey key,
            [NotNull] Func<TValue, TResult> valueSelector, [NotNull] Func<TResult> defaultValueFactory)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (valueSelector == null) throw new ArgumentNullException(nameof(valueSelector));
            if (defaultValueFactory == null) throw new ArgumentNullException(nameof(defaultValueFactory));

            return dict.TryGetValue(key, out TValue value) ? valueSelector(value) : defaultValueFactory();
        }

        /// <summary>
        /// Get value from <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if one of arguments is null.</exception>
        public static TValue? GetValueOrNull<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, [NotNull] TKey key)
            where TValue : struct
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            return dict.TryGetValue(key, out TValue value) ? (TValue?)value : null;
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TValue GetValueOrAdd<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, [NotNull] TKey key, TValue value)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            if (!dict.TryGetValue(key, out TValue ret)) dict.Add(key, ret = value);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <returns></returns>
        public static TValue GetValueOrAdd<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, TKey key,
            [NotNull] Func<TValue> valueFactory)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (valueFactory == null) throw new ArgumentNullException(nameof(valueFactory));

            if (!dict.TryGetValue(key, out TValue ret)) dict.Add(key, ret = valueFactory());
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

            if (!dict.TryGetValue(key, out TValue ret)) dict.Add(key, ret = new TValue());
            return ret;
        }

        #endregion
        
        /// <summary>
        /// Add If key is not exists.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryAdd<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (dict.ContainsKey(key)) return false;
            dict.Add(key, value);
            return true;
        }

        /// <summary>
        /// Create a <see cref="IReadOnlyDictionary{TKey, TValue}"/> wrap for <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            return new ReadOnlyDictionary<TKey, TValue>(dict);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="item"></param>
        public static void Add<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, [NotNull] TValue item)
            where TValue : IGetKey<TKey>
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (item == null) throw new ArgumentNullException(nameof(item));

            dict.Add(item.GetKey(), item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="items"></param>
        public static void AddRange<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, [NotNull] IEnumerable<TValue> items)
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
#pragma warning disable IDE0016 // 使用 "throw" 表达式
            if (item == null) throw new ArgumentNullException(nameof(item));
#pragma warning restore IDE0016 // 使用 "throw" 表达式

            dict[item.GetKey()] = item;
        }
    }
}