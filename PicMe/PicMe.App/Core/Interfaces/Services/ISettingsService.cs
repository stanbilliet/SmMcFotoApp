namespace PicMe.App.Core.Interfaces.Services
{
    public interface ISettingsService
    {
        Task<bool> SaveSettingsAsync(string schoolName, string clientId, string clientSecret, string soapApiKey,
            string sender, string backupAccount, bool identification);
    }
}
