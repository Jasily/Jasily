using Windows.UI.Xaml;

namespace Jasily.ViewModels
{
    /// <summary>
    /// Default view model for <see cref="UIElement"/>.
    /// </summary>
    public class UIElementViewModel : BaseViewModel
    {
        private Visibility _visibility;

        public Visibility Visibility
        {
            get => this._visibility;
            set => this.SetPropertyRef(ref this._visibility, value);
        }
    }
}