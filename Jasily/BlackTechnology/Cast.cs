using System;
using System.Diagnostics;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Jasily.BlackTechnology
{
    public static class Cast
    {
        /// <summary>
        /// Cast avoid boxing.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDest"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static TDest To<TSource, TDest>(TSource item)
        {
            return Core<TSource, TDest>.Func(item);
        }

        private static class Core<TFrom, TTo>
        {
            [NotNull] public static readonly Func<TFrom, TTo> Func;

            static Core()
            {
                if (typeof(TTo).IsAssignableFrom(typeof(TFrom)))
                {
                    var p = Expression.Parameter(typeof(TFrom));
                    Func = Expression.Lambda<Func<TFrom, TTo>>(p, p).Compile();
                    return;
                }

                // def
                Func = ConvertChecked.Core<TFrom, TTo>.Func;
            }
        }
    }
}