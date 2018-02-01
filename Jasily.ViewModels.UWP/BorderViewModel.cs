using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using JetBrains.Annotations;

namespace Jasily.ViewModels
{
    /// <summary>
    /// Default view model for <see cref="Border"/>.
    /// </summary>
    public class BorderViewModel : FrameworkElementViewModel
    {
        private Thickness _padding;
        private CornerRadius _cornerRadius;
        private TransitionCollection _childTransitions;
        private Thickness _borderThickness;
        private Brush _borderBrush;
        private Brush _background;

        [PublicAPI]
        public Thickness Padding
        {
            get => this._padding;
            set => this.SetPropertyRef(ref this._padding, value);
        }

        [PublicAPI]
        public CornerRadius CornerRadius
        {
            get => this._cornerRadius;
            set => this.SetPropertyRef(ref this._cornerRadius, value);
        }

        [PublicAPI]
        public TransitionCollection ChildTransitions
        {
            get => this._childTransitions;
            set => this.SetPropertyRef(ref this._childTransitions, value);
        }

        [PublicAPI]
        public Thickness BorderThickness
        {
            get => this._borderThickness;
            set => this.SetPropertyRef(ref this._borderThickness, value);
        }

        [PublicAPI]
        public Brush BorderBrush
        {
            get => this._borderBrush;
            set => this.SetPropertyRef(ref this._borderBrush, value);
        }

        [PublicAPI]
        public Brush Background
        {
            get => this._background;
            set => this.SetPropertyRef(ref this._background, value);
        }
    }
}