using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicMe.App.Core.Interfaces.Repositories;
using PicMe.App.Core.Interfaces.Services;
using PicMe.App.Views;
using System.Collections.ObjectModel;


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
              Toast.ToastShowter.ToastAlertAsync("Gelieve een school in te vullen!");

                hasError = true;
                return hasError;
            }

            if (string.IsNullOrWhiteSpace(clientIdEntry))
            {
                Toast.ToastShowter.ToastAlertAsync("Gelieve een client id in te vullen!");

                hasError = true;
                return hasError;
            }

            if (string.IsNullOrWhiteSpace(clientSecretEntry))
            {
                Toast.ToastShowter.ToastAlertAsync("Gelieve een client secret in te vullen!");

                hasError = true;
                return hasError;
            }

            if (string.IsNullOrWhiteSpace(apiKeyEntry))
            {
                Toast.ToastShowter.ToastAlertAsync("Gelieve een api key in te vullen!");

                hasError = true;
                return hasError;
            }

            if (string.IsNullOrWhiteSpace(senderEntry))
            {
                Toast.ToastShowter.ToastAlertAsync("Gelieve een sender in te vullen!");

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
            if (CheckFields())
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
                await Toast.ToastShowter.ToastAlertAsync("Instellingen zijn opgeslagen!");


                await Task.Delay(3000);

                await Toast.ToastShowter.ToastAlertAsync("Je wordt doorverwezen naar de Sync pagina.");

                await Shell.Current.GoToAsync($"//{nameof(SyncPage)}");
            }
            else
            {
                await Toast.ToastShowter.ToastAlertAsync("Het opslaan is mislukt.");

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
