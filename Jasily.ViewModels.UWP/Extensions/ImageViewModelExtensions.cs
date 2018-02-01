using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using JetBrains.Annotations;

namespace Jasily.ViewModels.Extensions
{
    public static class ImageViewModelExtensions
    {
        public static async Task SetSourceAsync([NotNull] this ImageViewModel viewModel, [NotNull] StorageFile storageFile)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            if (storageFile == null) throw new ArgumentNullException(nameof(storageFile));

            var bitmapImage = new BitmapImage();
            var stream = await storageFile.OpenAsync(FileAccessMode.Read);
            await bitmapImage.SetSourceAsync(stream);
            viewModel.Source = bitmapImage;
        }
    }
}