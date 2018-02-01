using Windows.UI.Xaml.Controls;

namespace Jasily.ViewModels
{
    /// <summary>
    /// Default view model for <see cref="ContentControl"/>.
    /// </summary>
    public class ContentControlViewModel : ControlViewModel
    {
        private object _content;

        public object Content
        {
            get => this._content;
            set => this.SetPropertyRef(ref this._content, value);
        }
    }
}