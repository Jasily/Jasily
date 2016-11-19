using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jasily.Linq.Expressions.Cache;
using JetBrains.Annotations;

namespace Jasily.Cache
{
    public class Enum<T>
        where T : struct, IComparable, IFormattable
    {
        private readonly bool isFlags;
        private readonly EnumItem[] items;

        private class EnumItem
        {
            internal readonly ulong Value;

            internal readonly T Item;

            internal readonly string Name;

            public EnumItem(ulong value, T item, string name)
            {
                this.Value = value;
                this.Item = item;
                this.Name = name;
            }
        }

        private static ulong ToUlong(T e) => e.ConvertUnchecked<T, ulong>();

        public static Enum<T> Default { get; } = new Enum<T>();

        private Enum()
        {
            var ti = typeof(T).GetTypeInfo();
            if (!ti.IsEnum) throw new InvalidOperationException();
            this.isFlags = ti.GetCustomAttribute<FlagsAttribute>() != null;
            this.items = ((T[]) Enum.GetValues(typeof(T)))
                .Select(z => new EnumItem(ToUlong(z), z, z.ToString()))
                .OrderBy(z => z.Value)
                .ToArray();
        }

        private EnumItem TryGetEnumItem(T e)
        {
            var val = ToUlong(e);
            return this.items.FirstOrDefault(z => z.Value == val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="completeMatch">if set true, if not complete match, will return null.</param>
        /// <returns></returns>
        private IEnumerable<EnumItem> SplitFlagItems(T e, bool completeMatch)
        {
            if (!this.isFlags) throw new InvalidOperationException();

            var val = ToUlong(e);

            if (val == 0)
            {
                if (this.items.Length > 0 && this.items[0].Value == 0)
                {
                    return new[] { this.items[0] };
                }
                else
                {
                    return completeMatch ? null : Enumerable.Empty<EnumItem>();
                }
            }

            var matchs = new List<EnumItem>();
            foreach (var item in this.items.Reverse())
            {
                if (item.Value == 0) break;

                if ((item.Value & val) == item.Value)
                {
                    val -= item.Value;
                    matchs.Insert(0, item);
                }
            }
            return val != 0 && completeMatch ? null : matchs;
        }

        public bool IsDefined(T e) => this.TryGetEnumItem(e) != null;

        public IEnumerable<T> SplitFlags(T e, bool completeMatch = true)
            => this.SplitFlagItems(e, completeMatch)?.Select(z => z.Item);

        public string ToString(T e)
        {
            var value = this.isFlags
                ? this.SplitFlagItems(e, true)?.Select(z => z.Name).JoinAsString(", ")
                : this.TryGetEnumItem(e)?.Name;
            return value ?? e.ToString();
        }

        public IEnumerable<T> All() => this.items.Select(z => z.Item);

        public bool TryParse([NotNull] string value, StringComparison comparison, ref T result)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var matchs = this.items.Where(z => string.Equals(z.Name, value, comparison)).ToArray();
            if (matchs.Length != 0)
            {
                result = matchs[0].Item;
                return true;
            }

            result = default(T);
            return false;
        }
    }
}