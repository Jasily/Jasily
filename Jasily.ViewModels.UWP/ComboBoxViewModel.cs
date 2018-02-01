using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using JetBrains.Annotations;

namespace Jasily.ViewModels
{
    /// <summary>
    /// Default view model for <see cref="ComboBox"/>.
    /// </summary>
    public class ComboBoxViewModel : SelectorViewModel
    {
        private bool _isDropDownOpen;
        private double _maxDropDownHeight;
        private DataTemplate _headerTemplate;
        private string _placeholderText;
        private object _header;
        private LightDismissOverlayMode _lightDismissOverlayMode;
        private bool _isTextSearchEnabled;
        private ComboBoxSelectionChangedTrigger _selectionChangedTrigger;
        private Brush _placeholderForeground;

        [PublicAPI]
        public double MaxDropDownHeight
        {
            get => this._maxDropDownHeight;
            set => this.SetPropertyRef(ref this._maxDropDownHeight, value);
        }

        [PublicAPI]
        public bool IsDropDownOpen
        {
            get => this._isDropDownOpen;
            set => this.SetPropertyRef(ref this._isDropDownOpen, value);
        }

        [PublicAPI]
        public DataTemplate HeaderTemplate
        {
            get => this._headerTemplate;
            set => this.SetPropertyRef(ref this._headerTemplate, value);
        }

        [PublicAPI]
        public string PlaceholderText
        {
            get => this._placeholderText;
            set => this.SetPropertyRef(ref this._placeholderText, value);
        }

        [PublicAPI]
        public object Header
        {
            get => this._header;
            set => this.SetPropertyRef(ref this._header, value);
        }

        [PublicAPI]
        public LightDismissOverlayMode LightDismissOverlayMode
        {
            get => this._lightDismissOverlayMode;
            set => this.SetPropertyRef(ref this._lightDismissOverlayMode, value);
        }

        [PublicAPI]
        public bool IsTextSearchEnabled
        {
            get => this._isTextSearchEnabled;
            set => this.SetPropertyRef(ref this._isTextSearchEnabled, value);
        }

        [PublicAPI]
        public ComboBoxSelectionChangedTrigger SelectionChangedTrigger
        {
            get => this._selectionChangedTrigger;
            set => this.SetPropertyRef(ref this._selectionChangedTrigger, value);
        }

        [PublicAPI]
        public Brush PlaceholderForeground
        {
            get => this._placeholderForeground;
            set => this.SetPropertyRef(ref this._placeholderForeground, value);
        }
    }
}