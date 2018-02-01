using Windows.UI.Xaml.Controls.Primitives;
using JetBrains.Annotations;

namespace Jasily.ViewModels
{
    /// <summary>
    /// Default view model for <see cref="ToggleButton"/>.
    /// </summary>
    public class ToggleButtonViewModel : ButtonBaseViewModel
    {
        private bool _isThreeState;
        private bool? _isChecked;

        [PublicAPI]
        public bool IsThreeState
        {
            get => this._isThreeState;
            set => this.SetPropertyRef(ref this._isThreeState, value);
        }

        [PublicAPI]
        public bool? IsChecked
        {
            get => this._isChecked;
            set => this.SetPropertyRef(ref this._isChecked, value);
        }
    }
}