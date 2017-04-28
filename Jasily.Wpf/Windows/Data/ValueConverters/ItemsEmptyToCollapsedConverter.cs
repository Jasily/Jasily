using System;
using System.Collections;
using System.Linq;

#if WINDOWS_DESKTOP
using System.Windows;
using System.Windows.Controls;
using Jasily.Windows.Data.ValueConverters.Internal;
#elif WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Jasily.UI.Xaml.Data.ValueConverters.Internal;
#endif
#if WINDOWS_DESKTOP
namespace Jasily.Windows.Data.ValueConverters
#elif WINDOWS_UWP
namespace Jasily.UI.Xaml.Data.ValueConverters
#endif
{
    public class ItemsEmptyToCollapsedConverter : InvariantValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter)
        {
            var list = value as IList;
            var enumerable = value as IEnumerable;
            if (list == null && enumerable == null)
            {
                if (value is ItemsControl itemControl)
                {
                    list = itemControl.ItemsSource as IList;
                    enumerable = itemControl.ItemsSource as IEnumerable; // as IEnumerable is use for UWP.
                }
            }
            if (list != null)
            {
                return list.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
            }

            if (enumerable != null)
            {
                return enumerable.Any() ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }
    }
}