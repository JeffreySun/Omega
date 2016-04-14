using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Omega.Converters
{
    public class StorageImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //var storageFile = value as StorageFile;
            //if(storageFile == null)
            //{
            //    return null;
            //}

            //using (IRandomAccessStream fileStream = await storageFile.OpenAsync(FileAccessMode.Read))
            //{
            //    // Set the image source to the selected bitmap.
            //    BitmapImage bitmapImage = new BitmapImage();

            //    bitmapImage.SetSource(fileStream);
            //    return bitmapImage;
            //}
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
