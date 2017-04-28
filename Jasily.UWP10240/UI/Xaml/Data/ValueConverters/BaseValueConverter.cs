using System;
using JetBrains.Annotations;
using Windows.UI.Xaml.Data;

namespace Jasily.UI.Xaml.Data.ValueConverters
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseValueConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public abstract object Convert([CanBeNull] object value, [NotNull]  Type targetType,
            [CanBeNull] object parameter, [CanBeNull] string language);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public virtual object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}