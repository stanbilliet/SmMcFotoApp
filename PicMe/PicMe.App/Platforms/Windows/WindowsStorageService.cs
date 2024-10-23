using Newtonsoft.Json;
using PicMe.Core.Entities;
using PicMe.Core.Interfaces.Repositories;
using PicMe.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Notifications;

namespace PicMe.App.Platforms.Windows
{
    public class WindowsStorageService : IStorageService
    {
        private readonly IJsonService _jsonService;
        private readonly ISoapRepository _soapRepository;

        public WindowsStorageService(IJsonService jsonService, ISoapRepository soapRepository)
        {
            _jsonService = jsonService;
            _soapRepository = soapRepository;
        }

        public async Task<bool> SaveSmartschoolProfilePictureToStudentFolderAsync(StudentInfo studentInfo)
        {
            var base64picture = await _soapRepository.GetBase64ProfilePictureAsync(studentInfo.Identifier);

            string imageName = $"{Guid.NewGuid()}";
            await SaveImageToLocalFolder(base64picture, imageName, studentInfo);
            studentInfo.ProfilePicture = string.Empty;

            return true;
        }

        public async Task<bool> CreateFoldersForStudentsAsync(StudentInfo studentInfo)
        {
            try
            {
                StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
                StorageFolder picMeFolder = await picturesFolder.CreateFolderAsync("PicMe", CreationCollisionOption.OpenIfExists);
                StorageFolder studentFolder = await picMeFolder.CreateFolderAsync(studentInfo.Identifier.Trim(), CreationCollisionOption.OpenIfExists);

                return true;
            }
            catch (Exception ex)
            {
                ShowToastNotification($"Error creating folder: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CreateStudentJsonFile(List<StudentInfo> studentInfos)
        {
            var studentsInfoJson = JsonConvert.SerializeObject(studentInfos, Formatting.Indented);
            await _jsonService.SaveDataAsJsonAsync(studentsInfoJson, "students.json");

            return true;
        }

        public async Task<string> GetLatestPictureForStudentAsync(StudentInfo studentInfo)
        {
            StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
            StorageFolder picMeFolder = await picturesFolder.CreateFolderAsync("PicMe", CreationCollisionOption.OpenIfExists);
            StorageFolder studentFolder = await picMeFolder.CreateFolderAsync(studentInfo.Identifier.Trim(), CreationCollisionOption.OpenIfExists);

            var imageFiles = await studentFolder.GetFilesAsync();

            if (imageFiles.Count == 0)
            {
                return string.Empty;
            }

            var latestFile = imageFiles.OrderByDescending(file => file.DateCreated).FirstOrDefault();

            return latestFile?.Path;
        }
        public async Task<string> SaveImageToLocalFolder(string base64Image, string imageName, StudentInfo studentInfo)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64Image);

                StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
                StorageFolder picMeFolder = await picturesFolder.CreateFolderAsync("PicMe", CreationCollisionOption.OpenIfExists);
                StorageFolder studentFolder = await picMeFolder.CreateFolderAsync(studentInfo.Identifier.Trim(), CreationCollisionOption.OpenIfExists);

                StorageFile imageFile = await studentFolder.CreateFileAsync($"{imageName}.jpg", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteBytesAsync(imageFile, imageBytes);

                return imageFile.Path;
            }
            catch (Exception ex)
            {
                ShowToastNotification($"Error saving image for {studentInfo.FamilyName} {studentInfo.GivenName}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteStudentPictures()
        {
            try
            {
                StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
                StorageFolder picMeFolder = await picturesFolder.CreateFolderAsync("PicMe", CreationCollisionOption.OpenIfExists);

                var studentFolders = await picMeFolder.GetFoldersAsync();
                foreach (var studentFolder in studentFolders)
                {
                    await studentFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }

                return true;
            }
            catch (Exception ex)
            {
                ShowToastNotification($"Error deleting student pictures: {ex.Message}");
                return false;
            }
        }

        private void ShowToastNotification(string message)
        {
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            var toastText = toastXml.GetElementsByTagName("text")[0];
            toastText.AppendChild(toastXml.CreateTextNode(message));

            var toastNotification = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }
    }
}
