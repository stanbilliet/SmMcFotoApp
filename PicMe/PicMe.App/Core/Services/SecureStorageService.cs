using PicMe.App.Core.Interfaces.Services;

namespace PicMe.App.Core.Services
{
    public class SecureStorageService : ISecureStorageService
    {
        public async Task<string> GetAsync(string key)
        {
            return await SecureStorage.Default.GetAsync(key);
        }

        public async Task SetAsync(string key, string value)
        {
            await SecureStorage.Default.SetAsync(key, value);
        }
    }
}
