using Newtonsoft.Json;
using PicMe.App.Core.Entities;
using PicMe.App.Core.Interfaces.Repositories;
using PicMe.App.Core.Interfaces.Services;
using PicMe.App.Platforms.Android;
using CommunityToolkit.Maui.Alerts;
using Environment = Android.OS.Environment;
using CommunityToolkit.Maui.Core;

[assembly: Dependency(typeof(AndroidStorageService))]
namespace PicMe.App.Platforms.Android
{
    public class AndroidStorageService : IStorageService
    {
        private readonly IJsonService _jsonService;
        private readonly ISoapRepository _soapRepository;

        public AndroidStorageService(IJsonService jsonService, ISoapRepository soapRepository)
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


                string basePath = Path.Combine(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures).AbsolutePath, "PicMe");
                string studentFolderPath = Path.Combine(basePath, $"{studentInfo.Identifier.Trim()}");

                if (!Directory.Exists(studentFolderPath))
                {
                    Directory.CreateDirectory(studentFolderPath);
                }

                return true;

            }
            catch (Exception ex)
            {

                var toast = CommunityToolkit.Maui.Alerts.Toast.Make($"Er was een probleem met het aanmaken van de mappen {ex.Message}");
                await toast.Show();
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
            string basePath = Path.Combine(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures).AbsolutePath, "PicMe");

            if (!Directory.Exists(basePath))
            {

                //create
                Directory.CreateDirectory(basePath);

            }

            string studentFolderPath = Path.Combine(basePath, $"{studentInfo.Identifier.Trim()}");

            if (!Directory.Exists(studentFolderPath))
            {
                //create
                Directory.CreateDirectory(studentFolderPath);

            }

            var imageFiles = Directory.GetFiles(studentFolderPath);

            if (imageFiles.Length == 0)
            {
                return string.Empty;
            }

            var latestFile = imageFiles
                .Select(file => new FileInfo(file))
                .OrderByDescending(fileInfo => fileInfo.LastWriteTime)
                .FirstOrDefault();

            return latestFile.FullName;
        }

        public async Task<string> SaveImageToLocalFolder(string base64Image, string imageName, StudentInfo studentInfo)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64Image);

                string basePath = Path.Combine(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures).AbsolutePath, "PicMe");
                string studentFolderPath = Path.Combine(basePath, $"{studentInfo.Identifier.Trim()}");

                if (!Directory.Exists(studentFolderPath))
                {
                    Directory.CreateDirectory(studentFolderPath);
                }

                string filePath = Path.Combine(studentFolderPath, $"{imageName}.jpg");

                await File.WriteAllBytesAsync(filePath, imageBytes);

                return filePath;
            }
            catch (Exception ex)
            {
                var toast = CommunityToolkit.Maui.Alerts.Toast.Make($"Er was een probleem met het opslaan van de foto van {studentInfo.FamilyName} {studentInfo.GivenName}: {ex.Message}");
                await toast.Show();
                return null;
            }

        }

        public Task<bool> DeleteStudentPictures()
        {
            string basePath = Path.Combine(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures).AbsolutePath, "PicMe");

            //get all folders

            var studentFolders = Directory.GetDirectories(basePath);

            foreach (var studentFolder in studentFolders)
            {
                Directory.Delete(studentFolder, true);

            }

            return Task.FromResult(true);

        }
    }
}