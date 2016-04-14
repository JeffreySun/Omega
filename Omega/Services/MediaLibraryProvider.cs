using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Omega.Models;
using Template10.Utils;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;

namespace Omega.Services
{
    public class MediaLibraryProvider
    {
        public static MediaLibraryProvider Instance { get; }

        static MediaLibraryProvider()
        {
            Instance = Instance ?? new MediaLibraryProvider();
        }

        private MediaLibraryProvider()
        {
        }

        public async Task<ObservableCollection<MediaFolder>> GetPictureLibraryFoldersAsync()
        {
            var pictures = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Pictures);
            var flatternFolders = new List<MediaFolder>();
            await VisitFoldersAsync(pictures.Folders, flatternFolders);
            return flatternFolders.ToObservableCollection();
        }

        public async Task<ObservableCollection<MediaFile>> GetPicturesAsync(MediaFolder folder)
        {
            var pictureFiles = await folder.Storage.GetFilesAsync();
            var pictures = new List<MediaFile>(pictureFiles.Count);
            foreach (var picture in pictureFiles)
            {
                var thumbnail = await ToThumbnailImageAsync(picture);
                pictures.Add(new MediaFile(picture) { Thumbnail = thumbnail });
            }

            return pictures.ToObservableCollection();
        }

        public async Task<BitmapImage> ToThumbnailImageAsync(
            StorageFile file,
            uint defaultThumbnailSize = 150)
        {
            try
            {
                var thumbnailFile =
                    await file.GetScaledImageAsThumbnailAsync(ThumbnailMode.ListView, defaultThumbnailSize);
                var bitmapImage = new BitmapImage();
                bitmapImage.SetSource(thumbnailFile);
                return bitmapImage;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task VisitFoldersAsync(IEnumerable<StorageFolder> folders, IList<MediaFolder> results)
        {
            if (folders == null || !folders.Any())
            {
                return;
            }

            foreach (var folder in folders)
            {
                var subfolders = await folder.GetFoldersAsync();
                var currentFiles = await folder.GetFilesAsync();
                if (currentFiles.Any())
                {
                    results.Add(
                        new MediaFolder(folder)
                        {
                            Thumbnail = 
                                await ToThumbnailImageAsync(currentFiles.FirstOrDefault())
                        });
                }

                await VisitFoldersAsync(subfolders, results);
            }
        }
    }
}

