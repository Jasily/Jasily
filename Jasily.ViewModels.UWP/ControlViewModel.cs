using Windows.UI.Xaml.Controls;

namespace Jasily.ViewModels
{
    /// <summary>
    /// Default view model for <see cref="Control"/>.
    /// </summary>
    public class ControlViewModel : FrameworkElementViewModel
    {
        private bool _isEnabled;

        public bool IsEnabled
        {
            get => this._isEnabled;
            set => this.SetPropertyRef(ref this._isEnabled, value);
        }
    }
}