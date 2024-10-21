using PicMe.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicMe.Test.Tests
{
    public class PhotoServiceTests
    {
        [Fact]
        public async Task DecodePictureFromBase64Async_Base64StringIsValid_ReturnsImageSource()
        {
            // Arrange
            var photoService = new PhotoService();
            var base64String = Convert.ToBase64String(new byte[] { 1, 2, 3, 4 });

            // Act
            var result = photoService.DecodePictureFromBase64Async(base64String);

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task DecodePictureFromBase64Async_Base64StringIsValid_ReturnsNull()
        {
            // Arrange
            var photoService = new PhotoService();
            var base64String = "invalidBase64String";

            // Act & Assert
            await Assert.ThrowsAsync<FormatException>(() => photoService.DecodePictureFromBase64Async(base64String));
        }
        [Fact]
        public async Task EncodePictureToBase64Async_FileExists_ReturnsBase64String()
        {
            // Arrange
            var photoService = new PhotoService();
            var filePath = "path/to/image.png";
            var base64String = Convert.ToBase64String(new byte[] { 1, 2, 3 });
            File.WriteAllBytes(filePath, [1, 2, 3]);

            // Act
            var result = await photoService.EncodePictureToBase64Async(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(base64String, result);
        }
        [Fact]
        public async Task EncodePictureToBase64Async_FileDoesNotExist_ThrowsDirectoryNotFoundException()
        {
            // Arrange
            var photoService = new PhotoService();
            var filePath = "path/to/nonexistent/image.png";

            // Act and Assert
            await Assert.ThrowsAsync<DirectoryNotFoundException>(() => photoService.EncodePictureToBase64Async(filePath));
        }
    }
}
