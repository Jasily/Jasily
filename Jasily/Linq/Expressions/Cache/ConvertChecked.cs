using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Jasily.Linq.Expressions.Cache
{
    internal static class ConvertChecked<TFrom, TTo>
    {
        [NotNull]
        public static readonly Func<TFrom, TTo> Func;

        static ConvertChecked()
        {
            var p = Expression.Parameter(typeof(TFrom));
            var c = Expression.ConvertChecked(p, typeof(TTo));
            Func = Expression.Lambda<Func<TFrom, TTo>>(c, p).Compile();
        }
    }
}