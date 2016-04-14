using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Omega.Models;
using Omega.Services;
using Omega.Views;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Navigation;

namespace Omega.ViewModels
{
    public class AlbumViewModel : ViewModelBase
    {
        private readonly MediaLibraryProvider mediaLibrary;

        public AlbumViewModel()
        {
            if (DesignMode.DesignModeEnabled)
            {
                // designtime
            }
            else
            {
                mediaLibrary = MediaLibraryProvider.Instance;
            }
        }

        private MediaFolder album;
        public MediaFolder Album
        {
            get { return album; }
            set { Set(ref album, value); }
        }

        private ObservableCollection<MediaFile> pictures;
        public ObservableCollection<MediaFile> Pictures
        {
            get { return pictures; }
            set { Set(ref pictures, value); }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode,
            IDictionary<string, object> suspensionState)
        {
            Busy.SetBusy(true, "Loading...");

            if (suspensionState.ContainsKey(nameof(Album)))
            {
                Album = suspensionState[nameof(Album)] as MediaFolder;
            }

            if (Album == null)
            {
                var albumPath = parameter.ToString();
                Album = SessionState[albumPath] as MediaFolder;
            }

            Pictures = await mediaLibrary.GetPicturesAsync(Album);
            await Task.CompletedTask;
            Busy.SetBusy(false);
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, 
            bool suspending)
        {
            if(suspending)
            {
                suspensionState[nameof(Album)] = Album;
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }
    }
}