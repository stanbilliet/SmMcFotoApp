using PicMe.App.Core.Interfaces.Services;

namespace PicMe.App.Core.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISecureStorageService _secureStorageService;

        public SettingsService(ISecureStorageService secureStorageService)
        {
            _secureStorageService = secureStorageService;
        }
        public async Task<bool> SaveSettingsAsync(string schoolName, string clientId, string clientSecret, string soapApiKey,
            string sender, string backupAccount, bool identification)
        {

            try
            {
                await _secureStorageService.SetAsync("SchoolName", schoolName);
                await _secureStorageService.SetAsync("ClientId", clientId);
                await _secureStorageService.SetAsync("ClientSecret", clientSecret);
                await _secureStorageService.SetAsync("SoapApiKey", soapApiKey);
                await _secureStorageService.SetAsync("Sender", sender);
                await _secureStorageService.SetAsync("Identification", identification.ToString());
                await _secureStorageService.SetAsync("Backup", backupAccount);

                return true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"De gegevens konden door een onvoorziene fout niet worden opgeslaan FOUT: {ex}, probeer opnieuw", "OK");
                return false;
            }
        }
    }
}
