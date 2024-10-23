namespace PicMe.App.Core.Interfaces.Services
{
    public interface IPhotoService
    {
        Task<string> EncodePictureToBase64Async(string profilePicture);
        Task<ImageSource> DecodePictureFromBase64Async(string profilePicture);
    }
}
