using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Jasily.BlackTechnology
{
    public static class ConvertUnchecked
    {
        /// <summary>
        /// ConvertUnchecked avoid boxing.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDest"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static TDest Convert<TSource, TDest>(TSource item)
        {
            return Core<TSource, TDest>.Func(item);
        }

        internal static class Core<TFrom, TTo>
        {
            [NotNull] public static readonly Func<TFrom, TTo> Func;

            static Core()
            {
                var p = Expression.Parameter(typeof(TFrom));
                var c = Expression.Convert(p, typeof(TTo));
                Func = Expression.Lambda<Func<TFrom, TTo>>(c, p).Compile();
            }
        }
    }
}