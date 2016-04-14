using Template10.Mvvm;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Omega.Models
{
    public class MediaFolder : ViewModelBase
    {
        private readonly StorageFolder storageFolder;

        public MediaFolder(StorageFolder folder)
        {
            storageFolder = folder;
        }

        public StorageFolder Storage => storageFolder;
        public string DisplayName => storageFolder.DisplayName;
        public string Name => storageFolder.Name;
        public string Path => storageFolder.Path;

        private BitmapImage thumbnail;
        public BitmapImage Thumbnail
        {
            get { return thumbnail; }
            set { Set(ref thumbnail, value); }
        }
    }
}
