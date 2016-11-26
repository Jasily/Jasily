using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Jasily.ComponentModel
{
    public struct PropertySelector<T>
    {
        private readonly string name;

        private PropertySelector(string name)
        {
            this.name = name;
        }

        public static PropertySelector<TProperty> From<TProperty>([NotNull] Expression<Func<T, TProperty>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return new PropertySelector<T>(null).Select(selector);
        }

        public static PropertySelector<T> From() => From(z => z);

        public PropertySelector<TProperty> Select<TProperty>([NotNull] Expression<Func<T, TProperty>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var expression = selector.Body;
            var name = this.Visit(expression);
            return new PropertySelector<TProperty>(this.name == null ? name : this.name + "." + name);
        }

        public PropertySelector<TProperty> SelectMany<TProperty>([NotNull] Expression<Func<T, IEnumerable<TProperty>>> selectExpression)
        {
            if (selectExpression == null) throw new ArgumentNullException(nameof(selectExpression));
            var expression = selectExpression.Body;
            var name = this.Visit(expression);
            return new PropertySelector<TProperty>(Concat(this.name, name));
        }

        private string Visit([NotNull] Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Parameter:
                    return null;

                case ExpressionType.TypeAs:
                case ExpressionType.Convert:
                    return this.Visit((UnaryExpression)expression);

                case ExpressionType.ArrayLength:
                    return Concat(this.Visit((UnaryExpression)expression), "Length");

                case ExpressionType.MemberAccess:
                    return this.Visit((MemberExpression)expression);

                default:
                    throw new NotSupportedException();
            }
        }

        private static string Concat(string left, string right)
            => left == null ? right : (right == null ? left : left + "." + right);

        private string Visit([NotNull] UnaryExpression expression) => this.Visit(expression.Operand);

        private string Visit([NotNull] MemberExpression expression)
        {
            var parentName = this.Visit(expression.Expression);
            var memberName = expression.Member.Name;
            return Concat(parentName, memberName);
        }

        public static implicit operator string(PropertySelector<T> selector)
            => selector.ToString();

        public override string ToString() => this.name ?? string.Empty;
    }

    public static class PropertySelector
    {
        public static PropertySelector<TProperty> SelectProperty<T, TProperty>([CanBeNull] this T obj,
            [NotNull] Expression<Func<T, TProperty>> selector)
            => PropertySelector<T>.From(selector);

        public static PropertySelector<TProperty> SelectProperty<T, TProperty>(
            [NotNull] Expression<Func<T, TProperty>> selector)
            => PropertySelector<T>.From(selector);
    }
}