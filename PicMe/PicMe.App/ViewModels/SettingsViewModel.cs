using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicMe.App.Views;
using PicMe.Core.Interfaces.Repositories;
using PicMe.Core.Interfaces.Services;
using PicMe.Core.Services;


namespace PicMe.App.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;
        private readonly IPinService _pinService;
        private readonly ISoapRepository _soapRepository;

        [ObservableProperty]
        private ObservableCollection<string> identifications;

        [ObservableProperty]
        private bool selectedIdentifier;

        [ObservableProperty]
        private string backupEntry;

        [ObservableProperty]
        private bool isPickerEnabled;

        [ObservableProperty]
        private string backupPlaceholder;

        [ObservableProperty]
        private string successMessage;

        [ObservableProperty]
        private string schoolEntry;

        [ObservableProperty]
        private string clientIdEntry;

        [ObservableProperty]
        private string clientSecretEntry;

        [ObservableProperty]
        private string apiKeyEntry;

        [ObservableProperty]
        private string senderEntry;


        public SettingsViewModel(ISettingsService settingsService, IPinService pinService, ISoapRepository soapRepository)
        {
            _settingsService = settingsService;

            Identifications = new ObservableCollection<string>
            {
                "User Identifier",
                "Internal Number"
            };
            _pinService = pinService;
            _soapRepository = soapRepository;
        }

        private bool CheckFields()
        {
            bool hasError = false;

            if (string.IsNullOrWhiteSpace(schoolEntry))
            {
                var errorToast = CommunityToolkit.Maui.Alerts.Toast.Make
                    ("Gelieve een school in te vullen!", ToastDuration.Short, 14);
                errorToast.Show();
                hasError = true;
                return hasError;
            }

            if (string.IsNullOrWhiteSpace(clientIdEntry))
            {
                var errorToast = CommunityToolkit.Maui.Alerts.Toast.Make
                    ("Gelieve een client id in te vullen!", ToastDuration.Short, 14);
                errorToast.Show();
                hasError = true;
                return hasError;
            }

            if (string.IsNullOrWhiteSpace(clientSecretEntry))
            {
                var errorToast = CommunityToolkit.Maui.Alerts.Toast.Make
                    ("Gelieve een client secret in te vullen!", ToastDuration.Short, 14);
                errorToast.Show();
                hasError = true;
                return hasError;
            }

            if (string.IsNullOrWhiteSpace(apiKeyEntry))
            {
                var errorToast = CommunityToolkit.Maui.Alerts.Toast.Make
                    ("Gelieve een api key in te vullen!", ToastDuration.Short, 14);
                errorToast.Show();
                hasError = true;
                return hasError;
            }

            if (string.IsNullOrWhiteSpace(senderEntry))
            {
                var errorToast = CommunityToolkit.Maui.Alerts.Toast.Make
                    ("Gelieve een sender in te vullen!", ToastDuration.Short, 14);
                errorToast.Show();
                hasError = true;
                return hasError;
            }

            return hasError;
        }

        [RelayCommand]
        private async Task ChangePin()
        {
            var viewModel = new PinCodeModalViewModel(_pinService, _soapRepository)
            {
                IsFirstTimeSetup = false,
                IsChangingPin = true
            };

            var pinCodeModalPage = new PinCodeModalPage(_pinService, _soapRepository) { BindingContext = viewModel };
            await Shell.Current.Navigation.PushModalAsync(pinCodeModalPage);
        }

        [RelayCommand]
        private async Task SaveSettings()
        {
            if(CheckFields())
            {
                return;
            }

            bool isSaved = await _settingsService.SaveSettingsAsync(
                SchoolEntry,
                ClientIdEntry,
                ClientSecretEntry,
                ApiKeyEntry,
                SenderEntry,
                BackupEntry,
                SelectedIdentifier);

            if (isSaved)
            {
                var successToast = CommunityToolkit.Maui.Alerts.Toast.Make("Instellingen zijn opgeslagen!", ToastDuration.Short, 14);
                await successToast.Show();

                await Task.Delay(3000);

                var redirectToast = CommunityToolkit.Maui.Alerts.Toast.Make("Je wordt doorverwezen naar de Sync pagina.", ToastDuration.Short, 14);
                await redirectToast.Show();

                await Shell.Current.GoToAsync($"//{nameof(SyncPage)}");
            }
            else
            {
                var errorToast = CommunityToolkit.Maui.Alerts.Toast.Make("Het opslaan is mislukt.", ToastDuration.Short, 14);
                await errorToast.Show();
            }

        }

        private void UpdateBackupEntryProperties()
        {
            if (SelectedIdentifier)
            {
                BackupPlaceholder = "Voer hier uw intern nummer in!";
            }
            else
            {
                BackupPlaceholder = "Voer hier uw user identifier in!";
            }
        }

        private void UpdatePickerState()
        {
            if (!string.IsNullOrWhiteSpace(BackupEntry))
            {
                if (SelectedIdentifier && !BackupEntry.All(char.IsDigit))
                {
                    BackupEntry = string.Empty;
                }

                IsPickerEnabled = false;
            }
            else
            {
                IsPickerEnabled = true;
            }
        }

        public async Task LoadSettingsAsync()
        {
            SchoolEntry = await SecureStorage.GetAsync("SchoolName") ?? string.Empty;
            ClientIdEntry = await SecureStorage.GetAsync("ClientId") ?? string.Empty;
            ClientSecretEntry = await SecureStorage.GetAsync("ClientSecret") ?? string.Empty;
            ApiKeyEntry = await SecureStorage.GetAsync("SoapApiKey") ?? string.Empty;
            SenderEntry = await SecureStorage.GetAsync("Sender") ?? string.Empty;
            var identificationString = await SecureStorage.GetAsync("Identification");
            SelectedIdentifier = bool.TryParse(identificationString, out var identification) && identification;
            BackupEntry = await SecureStorage.GetAsync("Backup") ?? string.Empty;
        }

    }
}
