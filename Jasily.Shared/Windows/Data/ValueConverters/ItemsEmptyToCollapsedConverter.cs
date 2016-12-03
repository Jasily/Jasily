using System;
using System.Collections;
using System.Linq;

#if WINDOWS_DESKTOP
using System.Windows;
using System.Windows.Controls;
using Jasily.Windows.Data.ValueConverters.Internal;

namespace Jasily.Windows.Data.ValueConverters
#elif WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Jasily.UI.Xaml.Data.ValueConverters.Internal;

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
                var itemControl = value as ItemsControl;
                if (itemControl != null)
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