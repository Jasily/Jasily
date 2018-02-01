using Windows.UI.Xaml;

namespace Jasily.ViewModels
{
    /// <summary>
    /// Default view model for <see cref="FrameworkElement"/>.
    /// </summary>
    public class FrameworkElementViewModel : UIElementViewModel
    {
        private object _dataContext;

        public object DataContext
        {
            get => this._dataContext;
            set => this.SetPropertyRef(ref this._dataContext, value);
        }
    }
}