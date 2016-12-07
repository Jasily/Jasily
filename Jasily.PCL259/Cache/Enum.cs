using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jasily.Linq.Expressions.Cache;
using JetBrains.Annotations;

namespace Jasily.Cache
{
    public static class Enum<T>
        where T : struct, IComparable, IFormattable
    {
        /// <summary>
        /// if count of enum items large then this count, then use Map to get item.
        /// </summary>
        private const int Threshold = 10;
        // ReSharper disable once StaticMemberInGenericType
        private static readonly string Name;
        // ReSharper disable once StaticMemberInGenericType
        private static readonly bool IsFlags;
        private static readonly EnumItem[] Items;
        private static readonly Dictionary<ulong, EnumItem> ItemsMap;

        private class EnumItem
        {
            /// <summary>
            /// the actual member value that enum define.
            /// </summary>
            internal readonly ulong EnumValue;

            /// <summary>
            /// 
            /// </summary>
            internal readonly T Item;

            /// <summary>
            /// cached name of enum.
            /// </summary>
            internal readonly string EnumName;

            public EnumItem(ulong value, T item, string name)
            {
                this.EnumValue = value;
                this.Item = item;
                this.EnumName = name;
            }
        }

        private static ulong ToUlong(T e) => e.ConvertUnchecked<T, ulong>();

        static Enum()
        {
            var ti = typeof(T).GetTypeInfo();
            if (!ti.IsEnum) throw new InvalidOperationException($"{ti.Name} is NOT enum type.");

            Name = ti.Name;
            IsFlags = ti.GetCustomAttribute<FlagsAttribute>() != null;
            Items = ((T[]) Enum.GetValues(typeof(T)))
                .Select(z => new EnumItem(ToUlong(z), z, z.ToString()))
                .OrderBy(z => z.EnumValue)
                .ToArray();
            ItemsMap = Items.ToDictionary(z => z.EnumValue);
        }

        private static EnumItem TryGetEnumItem(T e)
        {
            var val = ToUlong(e);
            return Items.Length > Threshold
                ? ItemsMap.GetValueOrDefault(val)
                : Items.FirstOrDefault(z => z.EnumValue == val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="completeMatch">if set true, if not complete match, will return null.</param>
        /// <returns></returns>
        private static IEnumerable<EnumItem> SplitFlagItems(T e, bool completeMatch)
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
                    return completeMatch ? null : Enumerable.Empty<EnumItem>();
                }
            }

            var matchs = new List<EnumItem>();
            foreach (var item in Items.Reverse())
            {
                if (item.EnumValue == 0) break;

                if ((item.EnumValue & val) == item.EnumValue)
                {
                    val -= item.EnumValue;
                    matchs.Insert(0, item);
                }
            }
            return val != 0 && completeMatch ? null : matchs;
        }

        public static bool IsDefined(T e) => TryGetEnumItem(e) != null;

        /// <summary>
        /// if complete match, return null when match false.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="completeMatch"></param>
        /// <returns></returns>
        [CanBeNull]
        public static IEnumerable<T> SplitFlags(T e, bool completeMatch = true)
            => SplitFlagItems(e, completeMatch)?.Select(z => z.Item);

        public static string ToString(T e)
        {
            var value = IsFlags
                ? SplitFlagItems(e, true)?.Select(z => z.EnumName).JoinAsString(", ")
                : TryGetEnumItem(e)?.EnumName;
            return value ?? e.ToString();
        }

        /// <summary>
        /// get all enum from T.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<T> All() => Items.Select(z => z.Item);

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
    }
}