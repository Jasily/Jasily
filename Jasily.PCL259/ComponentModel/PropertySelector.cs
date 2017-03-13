using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Jasily.ComponentModel
{
    public class PropertySelector<T>
    {
        private readonly string propertyName;

        public static PropertySelector<T> First { get; } = new PropertySelector<T>();

        private PropertySelector()
            : this(null)
        {
        }

        private PropertySelector(string name)
        {
            this.propertyName = name;
        }

        public PropertySelector<TProperty> Select<TProperty>([NotNull] Expression<Func<T, TProperty>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var body = selector.Body;
            var name = this.Visit(body);
            return new PropertySelector<TProperty>(this.propertyName == null ? name : this.propertyName + "." + name);
        }

        public PropertySelector<TProperty> SelectMany<TProperty>([NotNull] Expression<Func<T, IEnumerable<TProperty>>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var body = selector.Body;
            var name = this.Visit(body);
            return new PropertySelector<TProperty>(Concat(this.propertyName, name));
        }

        private string Visit([NotNull] Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Parameter:
                    return null;

                case ExpressionType.TypeAs:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
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
        {
            return left == null ? right : (right == null ? left : left + "." + right);
        }

        private string Visit([NotNull] UnaryExpression expression) => this.Visit(expression.Operand);

        private string Visit([NotNull] MemberExpression expression)
        {
            var parentName = this.Visit(expression.Expression);
            var memberName = expression.Member.Name;
            return Concat(parentName, memberName);
        }

        public static implicit operator string(PropertySelector<T> selector)
            => selector.ToString();

        public override string ToString() => this.propertyName ?? string.Empty;
    }
}