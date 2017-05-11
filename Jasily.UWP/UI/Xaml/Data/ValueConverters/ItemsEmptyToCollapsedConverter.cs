using System;
using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Jasily.UI.Xaml.Data.ValueConverters
{
    public class ItemsEmptyToCollapsedConverter : BaseValueConverter
    {
        public override object Convert([CanBeNull] object value, [NotNull] Type targetType,
            [CanBeNull] object parameter, [CanBeNull] string language)
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