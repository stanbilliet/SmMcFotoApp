using Moq;
using PicMe.Core.Interfaces.Services;
using PicMe.Core.Services;
using System.Threading.Tasks;

namespace PicMe.Test.Tests
{
    public class SettingsServiceTests
    {
        private Mock<ISecureStorageService> _secureStorageServiceMock;
        private SettingsService _settingsService;
        public SettingsServiceTests()
        {
            _secureStorageServiceMock = new Mock<ISecureStorageService>();
            _settingsService = new SettingsService(_secureStorageServiceMock.Object);
        }
        [Fact]
        public async Task SaveSettingsAsync_ValidData_SavesSuccessfully()
        {
            // Arrange
            string schoolName = "TestSchool";
            string clientId = "TestClientId";
            string clientSecret = "TestClientSecret";
            string soapApiKey = "TestSoapApiKey";
            string sender = "TestSender";
            string backupAccount = "TestBackupAccount";
            bool identification = true;

            // Act
            var result = await _settingsService.SaveSettingsAsync(
                schoolName,
                clientId,
                clientSecret,
                soapApiKey,
                sender,
                backupAccount,
                identification);

            _secureStorageServiceMock.Verify(service => service.SetAsync("SchoolName", schoolName), Times.Once);
            _secureStorageServiceMock.Verify(service => service.SetAsync("ClientId", clientId), Times.Once);
            _secureStorageServiceMock.Verify(service => service.SetAsync("ClientSecret", clientSecret), Times.Once);
            _secureStorageServiceMock.Verify(service => service.SetAsync("SoapApiKey", soapApiKey), Times.Once);
            _secureStorageServiceMock.Verify(service => service.SetAsync("Sender", sender), Times.Once);
            _secureStorageServiceMock.Verify(service => service.SetAsync("Identification", identification.ToString()), Times.Once);
            _secureStorageServiceMock.Verify(service => service.SetAsync("Backup", backupAccount), Times.Once);

            // Assert
            Assert.True(result);
        }

    }
}
