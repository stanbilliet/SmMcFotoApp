using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using PicMe.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicMe.Core.Services
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
                await Toast.Make($"Afbeelding in verkeerd formaat {ex.InnerException?.Message}", ToastDuration.Short).Show();
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
                await Toast.Make($"Afbeelding in verkeerd formaat {ex.InnerException.Message}", ToastDuration.Short).Show();
                return null;
            }
        }
    }
}
