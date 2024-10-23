using CommunityToolkit.Maui.Core;
using PicMe.App.Core.Interfaces.Services;

namespace PicMe.App.Core.Services
{
    public class PhotoService : IPhotoService
    {
        public async Task<ImageSource> DecodePictureFromBase64Async(string profilePicture)
        {
            try
            {
                if (!string.IsNullOrEmpty(profilePicture))
                {
                    var imageByteSize = Convert.FromBase64String(profilePicture);
                    return ImageSource.FromStream(() => new MemoryStream(imageByteSize));
                }
                return null;
            }
            catch (BadImageFormatException ex)
            {
                await CommunityToolkit.Maui.Alerts.Toast.Make($"Afbeelding in verkeerd formaat {ex.InnerException?.Message}", ToastDuration.Short).Show();
                return null;
            }
        }

        public async Task<string> EncodePictureToBase64Async(string profilePicture)
        {
            try
            {
                byte[] imageBytes = await File.ReadAllBytesAsync(profilePicture);
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
            catch (BadImageFormatException ex)
            {
                await CommunityToolkit.Maui.Alerts.Toast.Make($"Afbeelding in verkeerd formaat {ex.InnerException.Message}", ToastDuration.Short).Show();
                return null;
            }
        }
    }
}
