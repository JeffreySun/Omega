using System;
using Template10.Common;
using Template10.Services.SettingsService;
using Template10.Utils;
using Windows.UI.Xaml;

namespace Omega.Services
{
    public class SettingsService
    {
        private readonly ISettingsHelper helper;

        public static SettingsService Instance { get; }
        static SettingsService()
        {
            // implement singleton pattern
            Instance = Instance ?? new SettingsService();
        }

        private SettingsService()
        {
            helper = new SettingsHelper();
        }

        public bool UseShellBackButton
        {
            get { return helper.Read(nameof(UseShellBackButton), true); }
            set
            {
                helper.Write(nameof(UseShellBackButton), value);
                BootStrapper.Current.NavigationService.Dispatcher.Dispatch(() =>
                {
                    BootStrapper.Current.ShowShellBackButton = value;
                    BootStrapper.Current.UpdateShellBackButton();
                    BootStrapper.Current.NavigationService.Refresh();
                });
            }
        }

        public ApplicationTheme AppTheme
        {
            get
            {
                var theme = ApplicationTheme.Light;
                var value = helper.Read(nameof(AppTheme), theme.ToString());
                return Enum.TryParse(value, out theme) ? theme : ApplicationTheme.Dark;
            }
            set
            {
                helper.Write(nameof(AppTheme), value.ToString());
                var frame = Window.Current.Content as FrameworkElement;
                if(frame != null)
                {
                    frame.RequestedTheme = value.ToElementTheme();
                }

                Views.Shell.HamburgerMenu.RefreshStyles(value);
            }
        }

        public TimeSpan CacheMaxDuration
        {
            get
            {
                return helper.Read(nameof(CacheMaxDuration), TimeSpan.FromDays(2));
            }
            set
            {
                helper.Write(nameof(CacheMaxDuration), value);
                BootStrapper.Current.CacheMaxDuration = value;
            }
        }
    }
}