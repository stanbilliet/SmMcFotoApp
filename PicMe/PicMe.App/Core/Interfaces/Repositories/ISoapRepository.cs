namespace PicMe.App.Core.Interfaces.Repositories
{
    public interface ISoapRepository
    {
        Task<string> GetBase64ProfilePictureAsync(string userIdentifier);
        Task<string> SendPhotoAsync(string userIdentifier, string title, string body, string fileName, string fileData);
        Task<string> SetAccountPhotoAsync(string base64String, string userIdentifier);
    }
}
