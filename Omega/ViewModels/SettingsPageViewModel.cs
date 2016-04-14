using System;
using System.Threading.Tasks;
using Omega.Services;
using Template10.Mvvm;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace Omega.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();
    }

    public class SettingsPartViewModel : ViewModelBase
    {
        private readonly SettingsService settings;

        public SettingsPartViewModel()
        {
            if (DesignMode.DesignModeEnabled)
            {
                // designtime
            }
            else
            {
                settings = SettingsService.Instance;
            }
        }

        public bool UseShellBackButton
        {
            get { return settings.UseShellBackButton; }
            set
            {
                settings.UseShellBackButton = value;
                RaisePropertyChanged();
            }
        }

        public bool UseLightThemeButton
        {
            get { return settings.AppTheme.Equals(ApplicationTheme.Light); }
            set
            {
                settings.AppTheme = value ? ApplicationTheme.Light : ApplicationTheme.Dark;
                RaisePropertyChanged();
            }
        }

        private string busyText = "Please wait...";
        public string BusyText
        {
            get { return busyText; }
            set
            {
                Set(ref busyText, value);
                showBusyCommand.RaiseCanExecuteChanged();
            }
        }

        DelegateCommand showBusyCommand;
        public DelegateCommand ShowBusyCommand
            => showBusyCommand ?? (showBusyCommand = new DelegateCommand(async () =>
            {
                Views.Busy.SetBusy(true, busyText);
                await Task.Delay(5000);
                Views.Busy.SetBusy(false);
            }, () => !string.IsNullOrEmpty(BusyText)));
    }

    public class AboutPartViewModel : ViewModelBase
    {
        public Uri Logo => Package.Current.Logo;

        public string DisplayName => Package.Current.DisplayName;

        public string Publisher => Package.Current.PublisherDisplayName;

        public string Version
        {
            get
            {
                var v = Windows.ApplicationModel.Package.Current.Id.Version;
                return $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
            }
        }

        public Uri RateMe => new Uri("http://aka.ms/template10");
    }
}

