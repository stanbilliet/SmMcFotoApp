using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicMe.Core.Helpers
{
    public static class Base64Images
    {
        public static ImageSource Base64ToImage(string image)
        {
            try
            {
                if (image is not null && !string.IsNullOrEmpty(image))
                {
                    var base64String = image.Split(',', 2);
                    var imageByteSize = Convert.FromBase64String(base64String.LastOrDefault());
                    using MemoryStream memoryStream = new MemoryStream(imageByteSize);
                    return ImageSource.FromStream( () => new MemoryStream(imageByteSize));
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        public static async Task<string> ConvertImageToBase64(string filePath)
        {
            try
            {
                byte[] imageBytes = await File.ReadAllBytesAsync(filePath);
                string base64String = Convert.ToBase64String(imageBytes);

                return base64String;
            }
            catch (Exception)
            {
            }
            return null;
        }
    }
}
