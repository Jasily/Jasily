using System;
using System.Collections.Generic;

namespace Jasily.Collections.Generic
{
    public static class NameValuePair
    {
        public static NameValuePair<T> WithName<T>(this T obj, string name) => new NameValuePair<T>(name, obj);
    }

    public struct NameValuePair<TValue> : IEquatable<NameValuePair<TValue>>, IEquatable<NameValuePair<string, TValue>>
    {
        public NameValuePair(string name, TValue value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; }

        public TValue Value { get; }

        /// <summary>
        /// 指示当前对象是否等于同一类型的另一个对象。
        /// </summary>
        /// <returns>
        /// 如果当前对象等于 <paramref name="other"/> 参数，则为 true；否则为 false。
        /// </returns>
        /// <param name="other">与此对象进行比较的对象。</param>
        public bool Equals(NameValuePair<TValue> other) => EqualityComparer<TValue>.Default.Equals(this.Value, other.Value);

        bool IEquatable<NameValuePair<string, TValue>>.Equals(NameValuePair<string, TValue> other) => this.Equals(other);

        public override string ToString() => this.Name;

        public static implicit operator NameValuePair<TValue>(NameValuePair<string, TValue> value)
            => new NameValuePair<TValue>(value.Name, value.Value);
    }

    public struct NameValuePair<TName, TValue> : IEquatable<NameValuePair<TName, TValue>>
    {
        public NameValuePair(TName name, TValue value)
        {
            this.Name = name;
            this.Value = value;
        }

        public TName Name { get; }

        public TValue Value { get; }

        /// <summary>
        /// 指示当前对象是否等于同一类型的另一个对象。
        /// </summary>
        /// <returns>
        /// 如果当前对象等于 <paramref name="other"/> 参数，则为 true；否则为 false。
        /// </returns>
        /// <param name="other">与此对象进行比较的对象。</param>
        public bool Equals(NameValuePair<TName, TValue> other) => EqualityComparer<TValue>.Default.Equals(this.Value, other.Value);

        public override string ToString() => this.Name?.ToString();
    }
}