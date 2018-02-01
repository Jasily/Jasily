using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using JetBrains.Annotations;

namespace Jasily.ViewModels
{
    /// <summary>
    /// Default view model for <see cref="Panel"/>.
    /// </summary>
    public class PanelViewModel : FrameworkElementViewModel
    {
        private Brush _background;

        [PublicAPI]
        public Brush Background
        {
            get => this._background;
            set => this.SetPropertyRef(ref this._background, value);
        }
    }
}