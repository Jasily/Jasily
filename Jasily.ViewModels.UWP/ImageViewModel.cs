using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using JetBrains.Annotations;

namespace Jasily.ViewModels
{
    /// <summary>
    /// Default view model for <see cref="Image"/>.
    /// </summary>
    public class ImageViewModel : FrameworkElementViewModel
    {
        private Stretch _stretch;
        private ImageSource _source;
        private Thickness _nineGrid;

        [PublicAPI]
        public Stretch Stretch
        {
            get => this._stretch;
            set => this.SetPropertyRef(ref this._stretch, value);
        }

        [PublicAPI]
        public ImageSource Source
        {
            get => this._source;
            set => this.SetPropertyRef(ref this._source, value);
        }

        public Thickness NineGrid
        {
            get => this._nineGrid;
            set => this.SetPropertyRef(ref this._nineGrid, value);
        }
    }
}