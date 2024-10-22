using PicMe.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicMe.Core.Interfaces.Services
{
    public interface IStorageService
    {
        Task<string> SaveImageToLocalFolder(string base64Image, string imageName, StudentInfo studentInfo);
        Task<string> LoadImageFromLocalFolder(string imageName);
        Task<bool> CreateFoldersForStudentsAsync(StudentInfo studentInfo);
        Task<bool> CreateStudentJsonFile(List<StudentInfo> studentInfos);
        Task<bool> SaveSmartschoolProfilePictureToStudentFolderAsync(StudentInfo studentInfo);
        Task<bool> SaveImageToAppData(string pictureName, string base64ImageString);
        Task<string> GetLatestPictureForStudentAsync(StudentInfo studentInfo);
    }
}
