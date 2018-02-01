using Windows.UI.Xaml.Controls.Primitives;

namespace Jasily.ViewModels
{
    /// <summary>
    /// Default view model for <see cref="ButtonBase"/>.
    /// </summary>
    public class ButtonBaseViewModel : ContentControlViewModel
    {
        private object _commandParameter;

        public object CommandParameter
        {
            get => this._commandParameter;
            set => this.SetPropertyRef(ref this._commandParameter, value);
        }
    }
}