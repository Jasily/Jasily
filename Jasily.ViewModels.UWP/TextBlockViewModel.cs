using Windows.Media.PlayTo;
using Windows.UI.Xaml.Controls;
using JetBrains.Annotations;

namespace Jasily.ViewModels
{
    /// <summary>
    /// Default view model for <see cref="TextBlock"/>.
    /// </summary>
    public class TextBlockViewModel : FrameworkElementViewModel
    {
        private string _text;
        private string _selectedText;

        [PublicAPI]
        public string Text
        {
            get => this._text;
            set => this.SetPropertyRef(ref this._text, value);
        }

        [PublicAPI]
        public string SelectedText
        {
            get => this._selectedText;
            set => this.SetPropertyRef(ref this._selectedText, value);
        }
    }
}