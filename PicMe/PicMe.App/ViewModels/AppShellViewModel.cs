using CommunityToolkit.Mvvm.Input;
using PicMe.App.Core.Interfaces.Repositories;
using PicMe.App.Core.Interfaces.Services;
using PicMe.App.Views;

namespace PicMe.App.ViewModels
{
    public partial class AppShellViewModel(IPinService pinService, ISoapRepository soapRepository) : BaseViewModel
    {
        private const string PinCode = "PinCode";

        private readonly IPinService _pinService = pinService;
        private readonly ISoapRepository _soapRepository = soapRepository;

        public string VersionNumber => $"Version {AppInfo.VersionString}";


        [RelayCommand]
        public async Task NavigateToSettings()
        {
            var savedPin = await SecureStorage.GetAsync(nameof(PinCode));

            if (string.IsNullOrEmpty(savedPin))
            {
                var pinCodeModal = new PinCodeModalPage(_pinService, _soapRepository)
                {
                    BindingContext = new PinCodeModalViewModel(_pinService, _soapRepository) { IsFirstTimeSetup = true }
                };

                await Shell.Current.Navigation.PushModalAsync(pinCodeModal);
            }
            else
            {
                var pinCodeModal = new PinCodeModalPage(_pinService, _soapRepository)
                {
                    BindingContext = new PinCodeModalViewModel(_pinService, _soapRepository) { IsFirstTimeSetup = false }
                };

                await Shell.Current.Navigation.PushModalAsync(pinCodeModal);
            }
        }

        [RelayCommand]
        private async Task SetAppLanguage(string language)
        {
            SelectedLanguage = language;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage = new AppShell(new AppShellViewModel(_pinService, _soapRepository));
            });
        }

        public override void OnLanguageChanged()
        {
            base.OnLanguageChanged();
            base.SelectedLanguage = SelectedLanguage;
        }
    }
}
