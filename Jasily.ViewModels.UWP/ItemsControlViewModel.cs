using Windows.UI.Xaml.Controls;

namespace Jasily.ViewModels
{
    /// <summary>
    /// Default view model for <see cref="ItemsControl"/>.
    /// </summary>
    public class ItemsControlViewModel : ControlViewModel
    {
        private object _itemsSource;

        public object ItemsSource
        {
            get => this._itemsSource;
            set => this.SetPropertyRef(ref this._itemsSource, value);
        }
    }
}