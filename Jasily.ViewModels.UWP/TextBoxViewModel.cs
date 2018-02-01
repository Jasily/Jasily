using Windows.UI.Xaml.Controls;

namespace Jasily.ViewModels
{
    /// <summary>
    /// Default view model for <see cref="TextBox"/>.
    /// </summary>
    public class TextBoxViewModel : ControlViewModel
    {
        private object _header;
        private string _selectedText;
        private string _text;

        public object Header
        {
            get => this._header;
            set => this.SetPropertyRef(ref this._header, value);
        }

        public string SelectedText
        {
            get => this._selectedText;
            set => this.SetPropertyRef(ref this._selectedText, value);
        }

        public string Text
        {
            get => this._text;
            set => this.SetPropertyRef(ref this._text, value);
        }
    }
}