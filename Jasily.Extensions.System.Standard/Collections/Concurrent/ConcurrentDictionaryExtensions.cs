using System;
using System.Collections.Concurrent;
using JetBrains.Annotations;

namespace Jasily.Extensions.System.Collections.Concurrent
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConcurrentDictionaryExtensions
    {
        /// <summary>
        /// The default write lock is big lock. this is key level lock.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="isLockerFunc"></param>
        /// <param name="lockerFactory"></param>
        /// <param name="valueFactory"></param>
        /// <returns></returns>
        public static TValue FastGetOrAdd<TKey, TValue>([NotNull] this ConcurrentDictionary<TKey, TValue> dictionary, TKey key,
            [NotNull] Func<TValue, bool> isLockerFunc, [NotNull] Func<TValue> lockerFactory, [NotNull] Func<TValue> valueFactory)
            where TValue : class
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (isLockerFunc == null) throw new ArgumentNullException(nameof(isLockerFunc));
            if (lockerFactory == null) throw new ArgumentNullException(nameof(lockerFactory));
            if (valueFactory == null) throw new ArgumentNullException(nameof(valueFactory));

            if (!dictionary.TryGetValue(key, out var value))
            {
                var locker = lockerFactory();
                lock (locker)
                {
                    value = dictionary.GetOrAdd(key, locker);
                    if (ReferenceEquals(value, locker))
                    {
                        value = valueFactory();
                        dictionary[key] = value;
                        return value;
                    }
                }
            }

            if (isLockerFunc(value))
            {
                lock (value) { }
                return dictionary.FastGetOrAdd(key, isLockerFunc, lockerFactory, valueFactory);
            }
            else
            {
                return value;
            }
        }

        public static TValue FastGetOrAdd<TKey, TValue>([NotNull] this ConcurrentDictionary<TKey, TValue> dictionary, TKey key,
            [NotNull] Func<TValue, bool> isLockerFunc, [NotNull] Func<TValue> lockerFactory, [NotNull] Func<TKey, TValue> valueFactory)
            where TValue : class
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (isLockerFunc == null) throw new ArgumentNullException(nameof(isLockerFunc));
            if (lockerFactory == null) throw new ArgumentNullException(nameof(lockerFactory));
            if (valueFactory == null) throw new ArgumentNullException(nameof(valueFactory));

            if (!dictionary.TryGetValue(key, out var value))
            {
                var locker = lockerFactory();
                lock (locker)
                {
                    value = dictionary.GetOrAdd(key, locker);
                    if (ReferenceEquals(value, locker))
                    {
                        value = valueFactory(key);
                        dictionary[key] = value;
                        return value;
                    }
                }
            }

            if (isLockerFunc(value))
            {
                lock (value) { }
                return dictionary.FastGetOrAdd(key, isLockerFunc, lockerFactory, valueFactory);
            }
            else
            {
                return value;
            }
        }
    }
}