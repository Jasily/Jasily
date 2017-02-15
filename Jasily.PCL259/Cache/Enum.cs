using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jasily.Core;
using Jasily.Linq.Expressions.Cache;
using JetBrains.Annotations;

namespace Jasily.Cache
{
    using E = System.Enum;

    internal static class Enum
    {
        /// <summary>
        /// if count of enum items large then this count, then use Map to get item.
        /// </summary>
        internal const int Threshold = 10;

        internal class EnumItem
        {
            internal static readonly Func<EnumItem, string> EnumNameSelector = z => z.EnumName;

            /// <summary>
            /// the actual member value that enum define.
            /// </summary>
            internal readonly ulong EnumValue;

            /// <summary>
            /// cached name of enum.
            /// </summary>
            internal readonly string EnumName;

            protected EnumItem(ulong value, string name)
            {
                this.EnumValue = value;
                this.EnumName = name;
            }
        }
    }

    public static class Enum<T>
        where T : struct, IComparable, IFormattable
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly string Name;
        // ReSharper disable once StaticMemberInGenericType
        private static readonly bool IsFlags;
        private static readonly GenericEnumItem[] Items;
        private static readonly Dictionary<ulong, GenericEnumItem> ItemsMap;

        private class GenericEnumItem : Enum.EnumItem
        {
            /// <summary>
            /// provide (z => z.Item) cache.
            /// </summary>
            // ReSharper disable once StaticMemberInGenericType
            internal static readonly Func<GenericEnumItem, T> ItemSelector = z => z.Item;

            /// <summary>
            /// 
            /// </summary>
            internal readonly T Item;

            public GenericEnumItem(ulong value, T item, string name)
                : base(value, name)
            {
                this.Item = item;
            }
        }

        private static ulong ToUlong(T e) => e.ConvertUnchecked<T, ulong>();

        private static T FromUlong(ulong e) => e.ConvertUnchecked<ulong, T>();

        static Enum()
        {
            var ti = typeof(T).GetTypeInfo();
            if (!ti.IsEnum) throw new InvalidOperationException($"{ti.Name} is NOT enum type.");

            Name = ti.Name;
            IsFlags = ti.GetCustomAttribute<FlagsAttribute>() != null;
            Items = ((T[]) E.GetValues(typeof(T)))
                .Select(z => new GenericEnumItem(ToUlong(z), z, z.ToString()))
                .OrderBy(z => z.EnumValue)
                .ToArray();
            ItemsMap = Items.ToDictionary(z => z.EnumValue);
        }

        private static GenericEnumItem TryGetEnumItem(T e)
        {
            var val = ToUlong(e);
            return Items.Length > Enum.Threshold
                ? ItemsMap.GetValueOrDefault(val)
                : Items.FirstOrDefault(z => z.EnumValue == val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="completeMatch">if set true, if not complete match, will return null.</param>
        /// <returns></returns>
        private static GenericEnumItem[] SplitFlagItems(T e, bool completeMatch)
        {
            if (!IsFlags) throw new InvalidOperationException();

            var val = ToUlong(e);

            if (val == 0)
            {
                if (Items.Length > 0 && Items[0].EnumValue == 0)
                {
                    return new[] { Items[0] };
                }
                else
                {
                    return completeMatch ? null : Empty<GenericEnumItem>.Array;
                }
            }

            var matchs = new List<GenericEnumItem>();
            foreach (var item in Items.Reverse())
            {
                if (item.EnumValue == 0) break;

                if ((item.EnumValue & val) == item.EnumValue)
                {
                    val -= item.EnumValue;
                    matchs.Insert(0, item);
                }
            }
            return val != 0 && completeMatch ? null : matchs.ToArray();
        }

        public static bool IsDefined(T e) => TryGetEnumItem(e) != null;

        /// <summary>
        /// if complete match, return null when match false.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="completeMatch"></param>
        /// <returns></returns>
        [CanBeNull]
        public static T[] SplitFlags(T e, bool completeMatch = true)
            => SplitFlagItems(e, completeMatch)?.ConvertToArray(GenericEnumItem.ItemSelector);

        public static string ToString(T e)
        {
            var value = IsFlags
                ? SplitFlagItems(e, true)?.Select(Enum.EnumItem.EnumNameSelector).JoinAsString(", ")
                : TryGetEnumItem(e)?.EnumName;
            return value ?? e.ToString();
        }

        /// <summary>
        /// get all enum from T.
        /// </summary>
        /// <returns></returns>
        public static T[] All() => Items.ConvertToArray(GenericEnumItem.ItemSelector);

        public static bool TryParse([NotNull] string value, bool ignoreCase, out T result)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            var matchs = Items.FirstOrDefault(z => string.Equals(z.EnumName, value, comparison));
            if (matchs != null)
            {
                result = matchs.Item;
                return true;
            }
            else
            {
                result = default(T);
                return false;
            }
        }

        public static string ToFullString(T value, string spliter = ".")
            => string.Concat(Name, spliter ?? ".", value.ToString());

        /// <summary>
        /// and op for enum without boxing.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static T And(T first, T second) => FromUlong(ToUlong(first) & ToUlong(second));

        /// <summary>
        /// or op for enum without boxing.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static T Or(T first, T second) => FromUlong(ToUlong(first) | ToUlong(second));
    }
}