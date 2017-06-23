using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Jasily.Linq.Expressions.Cache
{
    internal static class ConvertUnchecked<TFrom, TTo>
    {
        [NotNull]
        public static readonly Func<TFrom, TTo> Func;

        static ConvertUnchecked()
        {
            var p = Expression.Parameter(typeof(TFrom));
            var c = Expression.Convert(p, typeof(TTo));
            Func = Expression.Lambda<Func<TFrom, TTo>>(c, p).Compile();
        }
    }
}