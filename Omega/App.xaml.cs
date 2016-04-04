using System;
using System.Threading.Tasks;
using Omega.Services;
using Template10.Controls;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Omega
{
    sealed partial class App
    {
        public App()
        {
            InitializeComponent();
            SplashFactory = e => new Views.Splash(e);

            var settings = SettingsService.Instance;
            RequestedTheme = settings.AppTheme;
            CacheMaxDuration = settings.CacheMaxDuration;
            ShowShellBackButton = settings.UseShellBackButton;
        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            if (!(Window.Current.Content is ModalDialog))
            {
                // create a new frame 
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
                // create modal root
                Window.Current.Content = new ModalDialog
                {
                    DisableBackButtonWhenModal = true,
                    Content = new Views.Shell(nav),
                    ModalContent = new Views.Busy(),
                };
            }
            await Task.CompletedTask;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // long-running startup tasks go here

            NavigationService.Navigate(typeof(Views.AlbumsView));
            await Task.CompletedTask;
        }
    }
}

