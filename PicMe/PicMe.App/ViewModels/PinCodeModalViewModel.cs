using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicMe.App.Core.Interfaces.Repositories;
using PicMe.App.Core.Interfaces.Services;

namespace PicMe.App.ViewModels
{
    public partial class PinCodeModalViewModel(IPinService pinService, ISoapRepository soapRepository) : ObservableObject
    {
        [ObservableProperty]
        private string pinCode;

        [ObservableProperty]
        private string nameCheck;

        [ObservableProperty]
        private bool isEntryVisible;

        [ObservableProperty]
        private bool isEntryButtonVisible;

        [ObservableProperty]
        private string confirmPin;

        [ObservableProperty]
        private bool isResetPinVisible;

        private bool isFirstTimeSetup;

        public bool IsFirstTimeSetup
        {
            get { return isFirstTimeSetup; }
            set
            {
                SetProperty(ref isFirstTimeSetup, value);
                OnPropertyChanged(nameof(IsConfirmationVisiable));
                OnPropertyChanged(nameof(ModalLabel));
                UpdateResetPinVisibility();
            }
        }

        [ObservableProperty]
        private string modalLabel;

        private bool isChangingPin;

        public bool IsChangingPin
        {
            get { return isChangingPin; }
            set
            {
                SetProperty(ref isChangingPin, value);
                OnPropertyChanged(nameof(IsConfirmationVisiable));
                OnPropertyChanged(nameof(ModalLabel));
                UpdateResetPinVisibility();
            }
        }

        private bool isConfirmationVisiable;

        public bool IsConfirmationVisiable
        {
            get
            {
                if (IsFirstTimeSetup)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        private readonly IPinService _pinService = pinService;
        private readonly ISoapRepository _soapRepository = soapRepository;

        private void UpdateResetPinVisibility()
        {
            if (IsFirstTimeSetup || IsChangingPin)
            {
                IsResetPinVisible = false;
            }
            else
            {
                IsResetPinVisible = true;
            }
        }


        [RelayCommand]
        private async Task ConfirmPinCode()
        {
            var savedPinCode = await SecureStorage.GetAsync(nameof(PinCode));

            bool actionCompleted;

            if (IsFirstTimeSetup)
            {
                actionCompleted = await HandleFirstTimeSetupAsync(savedPinCode);
            }
            else if (IsChangingPin)
            {
                actionCompleted = await HandlePinChangeAsync(savedPinCode);
            }
            else
            {
                actionCompleted = await HandlePinVerificationAsync(savedPinCode);
            }

            if (actionCompleted)
            {
                await CloseModalAndNavigateToSettingsAsync();
            }
        }

        private async Task<bool> HandleFirstTimeSetupAsync(string savedPinCode)
        {
            if (await AreFieldsValidAsync())
            {
                if (PinCode == savedPinCode)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "De nieuwe pincode mag niet hetzelfde zijn als de oude pincode", "OK");
                    PinCode = string.Empty;
                    ConfirmPin = string.Empty;
                    return false;
                }
                else
                {
                    await SaveNewPinCodeAsync();
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> HandlePinChangeAsync(string savedPinCode)
        {
            if (PinCode != savedPinCode)
            {
                await App.Current.MainPage.DisplayAlert("Error", "De ingevoerde huidige pincode is onjuist", "OK");
                PinCode = string.Empty;
                return false;
            }
            else
            {
                IsFirstTimeSetup = true;
                IsChangingPin = false;
                PinCode = string.Empty;
                ConfirmPin = string.Empty;
                await App.Current.MainPage.DisplayAlert("Info", "Voer een nieuwe pincode in", "OK");
                return false;
            }
        }

        private async Task<bool> HandlePinVerificationAsync(string savedPinCode)
        {
            if (PinCode != savedPinCode)
            {
                await App.Current.MainPage.DisplayAlert("Error", "De pincode is onjuist", "OK");
                PinCode = string.Empty;
                return false;
            }
            else
            {
                Snack();
                return true;
            }
        }

        private async Task<bool> AreFieldsValidAsync()
        {
            if (string.IsNullOrEmpty(PinCode) || string.IsNullOrEmpty(ConfirmPin))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Gelieve beide velden in te vullen", "OK");
                PinCode = string.Empty;
                ConfirmPin = string.Empty;
                return false;
            }

            if (PinCode.Length != 4 || ConfirmPin.Length != 4)
            {
                await App.Current.MainPage.DisplayAlert("Error", "De pincode moet 4 cijfers lang zijn", "OK");
                PinCode = string.Empty;
                ConfirmPin = string.Empty;
                return false;
            }

            if (PinCode != ConfirmPin)
            {
                await App.Current.MainPage.DisplayAlert("Error", "De pincode komt niet overeen", "OK");
                PinCode = string.Empty;
                ConfirmPin = string.Empty;
                return false;
            }

            if (!int.TryParse(PinCode, out _) || !int.TryParse(ConfirmPin, out _))
            {
                await App.Current.MainPage.DisplayAlert("Error", "De pincode mag alleen cijfers bevatten", "OK");
                PinCode = string.Empty;
                ConfirmPin = string.Empty;
                return false;
            }

            return true;
        }

        private async Task SaveNewPinCodeAsync()
        {
            await SecureStorage.SetAsync(nameof(PinCode), PinCode);
            await App.Current.MainPage.DisplayAlert("Success", "Pincode is succesvol gewijzigd", "OK");
        }

        private async Task CloseModalAndNavigateToSettingsAsync()
        {
            await Shell.Current.Navigation.PopModalAsync();
            Shell.Current.FlyoutIsPresented = false;
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }

        public async void Snack()
        {
            await Toast.ToastShowter.ToastAlertAsync("Heel mooi");
        }

        [RelayCommand]
        public async Task SetNewPin()
        {
            var result = await Application.Current.MainPage.DisplayActionSheet("Pincode herstellen", null, null, "Ja", "Nee");

            if (result == "Ja")
            {
                IsEntryVisible = true;
                IsEntryButtonVisible = true;
            }
        }
        [RelayCommand]
        public async Task CheckValue()
        {
            var account = await SecureStorage.GetAsync("Backup");

            if (string.IsNullOrWhiteSpace(NameCheck))
            {
                await Application.Current.MainPage.DisplayAlert("Fout", "Vul a.u.b. de accountnaam in om verder te gaan", "OK");
                return;
            }

            if (NameCheck == account)
            {
                var pinResult = _pinService.SetNewPinAsync();
                if (pinResult != null)
                {
                    var savedPinCode = await SecureStorage.GetAsync(nameof(PinCode));
                    if (savedPinCode != null)
                    {
                        await SecureStorage.SetAsync(nameof(PinCode), pinResult.Result);
                    }

                    savedPinCode = await SecureStorage.GetAsync(nameof(PinCode));
                    await _soapRepository.SendPhotoAsync(account, "Pincode is gewijzigd", $"Dit is uw nieuwe pincode {savedPinCode}", null, null);
                    await Application.Current.MainPage.DisplayAlert("Success", "Pincode is succesvol gewijzigd \n en er werd een mail verstuurd met de pin naar het backup account", "OK");
                    IsEntryButtonVisible = false;
                    IsEntryVisible = false;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Fout", "Het ingevoerde account komt niet overeen met het backup account", "OK");
            }
        }
    }
}