using System;
using Windows.UI.Xaml;
using JetBrains.Annotations;

namespace Jasily.UI.Xaml.Data.ValueConverters
{
    public class StringToVisibilityValueConverter : BaseValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public override object Convert([CanBeNull] object value, [NotNull] Type targetType,
            [CanBeNull] object parameter, [CanBeNull] string language)
        {
            var boolValue = parameter as string == nameof(string.IsNullOrWhiteSpace)
                ? string.IsNullOrWhiteSpace(value as string)
                : string.IsNullOrEmpty(value as string);
            return boolValue ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}