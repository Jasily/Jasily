using System;
using System.Collections.Generic;

namespace Jasily.Converters
{
    public interface IValueConverter
    {
        IEnumerable<Type> ConvertSourceType { get; }

        bool CanConvertTo(Type sourceType, Type destType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destType"></param>
        /// <param name="value"></param>
        /// <exception cref="NotSupportedException"></exception>
        /// <returns></returns>
        bool TryConvert(object source, Type destType, out object value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destType"></param>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ConvertException"></exception>
        /// <returns></returns>
        object Convert(object source, Type destType);
    }

    public interface IValueConverter<in TSource> : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destType"></param>
        /// <param name="value"></param>
        /// <exception cref="NotSupportedException"></exception>
        /// <returns></returns>
        bool TryConvert(TSource source, Type destType, out object value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destType"></param>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ConvertException"></exception>
        /// <returns></returns>
        object Convert(TSource source, Type destType);
    }

    public interface IValueConverter<in TSource, TResult> : IValueConverter<TSource>
    {
        bool TryConvert(TSource value, out TResult result);

        TResult Convert(TSource value);
    }
}
