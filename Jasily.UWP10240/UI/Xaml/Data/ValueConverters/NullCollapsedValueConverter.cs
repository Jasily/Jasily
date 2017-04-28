using System;
using JetBrains.Annotations;
using Windows.UI.Xaml;
namespace Jasily.UI.Xaml.Data.ValueConverters
{
    public class NullCollapsedValueConverter : BaseValueConverter
    {
        public override object Convert([CanBeNull] object value, [NotNull] Type targetType, [CanBeNull] object parameter, [CanBeNull] string language)
        {
            return ReferenceEquals(value, null) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}