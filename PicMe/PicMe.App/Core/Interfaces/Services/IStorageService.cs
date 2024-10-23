using PicMe.App.Core.Entities;

namespace PicMe.App.Core.Interfaces.Services
{
    public interface IStorageService
    {
        Task<string> SaveImageToLocalFolder(string base64Image, string imageName, StudentInfo studentInfo);
        Task<bool> CreateFoldersForStudentsAsync(StudentInfo studentInfo);
        Task<bool> CreateStudentJsonFile(List<StudentInfo> studentInfos);
        Task<bool> SaveSmartschoolProfilePictureToStudentFolderAsync(StudentInfo studentInfo);
        Task<string> GetLatestPictureForStudentAsync(StudentInfo studentInfo);
        Task<bool> DeleteStudentPictures();
    }
}
