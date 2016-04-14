using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    public class LibraryViewModel : ViewModelBase
    {
        private readonly MediaLibraryProvider mediaLibrary;
        public LibraryViewModel()
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

        private ObservableCollection<MediaFolder> albums;
        public ObservableCollection<MediaFolder> Albums
        {
            get { return albums; }
            set { Set(ref albums, value); }
        }

        private DelegateCommand<MediaFolder> gotoAlbumCommand;

        public DelegateCommand<MediaFolder> GotoAlbumCommand
            => gotoAlbumCommand ?? (gotoAlbumCommand =
                new DelegateCommand<MediaFolder>(folder =>
                    {
                        var normalizedPath = folder.Path.ToLowerInvariant();
                        if(SessionState.ContainsKey(normalizedPath))
                        {
                            SessionState.Remove(normalizedPath);
                        }

                        SessionState.Add(normalizedPath, folder);
                        NavigationService.Navigate(typeof(AlbumView), normalizedPath);
                    },
                    folder => true));

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode,
            IDictionary<string, object> suspensionState)
        {
            Busy.SetBusy(true, "Loading...");

            if (suspensionState.Any())
            {
                Albums = suspensionState[nameof(Albums)] as ObservableCollection<MediaFolder>;
            }

            if(Albums == null || !Albums.Any())
            {
                Albums = await mediaLibrary.GetPictureLibraryFoldersAsync();
            }

            await Task.CompletedTask;
            Busy.SetBusy(false);
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState,
            bool suspending)
        {
            if (suspending)
            {
                suspensionState[nameof(Albums)] = Albums;
            }

            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }

        public void GotoDetailsPage() =>
            NavigationService.Navigate(typeof(AlbumView), "hahaha");

        public void GotoSettings() =>
            NavigationService.Navigate(typeof(SettingsPage), 0);

        public void GotoPrivacy() =>
            NavigationService.Navigate(typeof(SettingsPage), 1);

        public void GotoAbout() =>
            NavigationService.Navigate(typeof(SettingsPage), 2);
    }
}

