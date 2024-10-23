using PicMe.App.Core.Interfaces.Services;

namespace PicMe.App.Core.Services
{
    public class PinService : IPinService
    {
        public async Task<string> SetNewPinAsync()
        {
            Random random = new Random();

            string newPin = "";
            for (int i = 0; i < 4; i++)
            {
                newPin += random.Next(0, 10).ToString();
            }

            return await Task.FromResult(newPin);
        }
    }
}
