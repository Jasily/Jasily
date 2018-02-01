using Windows.UI.Xaml.Controls.Primitives;
using JetBrains.Annotations;

namespace Jasily.ViewModels
{
    /// <summary>
    /// Default view model for <see cref="Selector"/>.
    /// </summary>
    public class SelectorViewModel : ItemsControlViewModel
    {
        private object _selectedValue;
        private object _selectedItem;
        private int _selectedIndex;

        [PublicAPI]
        public object SelectedValue
        {
            get => this._selectedValue;
            set => this.SetPropertyRef(ref this._selectedValue, value);
        }

        [PublicAPI]
        public object SelectedItem
        {
            get => this._selectedItem;
            set => this.SetPropertyRef(ref this._selectedItem, value);
        }

        [PublicAPI]
        public int SelectedIndex
        {
            get => this._selectedIndex;
            set => this.SetPropertyRef(ref this._selectedIndex, value);
        }
    }
}