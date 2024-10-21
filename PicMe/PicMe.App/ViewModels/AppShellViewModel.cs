using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicMe.App.Resources.Strings;
using PicMe.App.Views;
using PicMe.Core.Interfaces.Repositories;
using PicMe.Core.Interfaces.Services;
using PicMe.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PicMe.App.ViewModels
{
    public partial class AppShellViewModel(IPinService pinService, ISoapRepository soapRepository) : BaseViewModel
    {
        private const string PinCode = "PinCode";

        private readonly IPinService _pinService = pinService;
        private readonly ISoapRepository _soapRepository = soapRepository;

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
