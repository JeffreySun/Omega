using Template10.Mvvm;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Omega.Models
{
    public class MediaFile : ViewModelBase
    {
        private readonly StorageFile storageFile;

        public MediaFile(StorageFile file)
        {
            storageFile = file;
        }

        public string DisplayName => storageFile.DisplayName;
        public string Path => storageFile.Path;

        private BitmapImage thumbnail;
        public BitmapImage Thumbnail
        {
            get { return thumbnail; }
            set { Set(ref thumbnail, value); }
        }
    }
}
