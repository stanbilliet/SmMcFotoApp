using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using PicMe.App.Platforms.Windows;
using PicMe.Core.Entities;
using PicMe.Core.Interfaces.Repositories;
using PicMe.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Dependency(typeof(WindowsStorageService))]
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

        public async Task<bool> SaveSmartschoolProfilePictureToStudentFolderAsync()
        {
            var studentsData = await _jsonService.ReadDataFromJsonAsync("students.json");
            var studentsInfo = JsonConvert.DeserializeObject<List<StudentInfo>>(studentsData);

            foreach (var studentInfo in studentsInfo)
            {
                var base64picture = await _soapRepository.GetBase64ProfilePictureAsync(studentInfo.Identifier);
                string date = DateTime.Now.ToShortDateString();
                string dateWithoutSlahes = date.Replace("/", "");
                await SaveImageToLocalFolder(base64picture, $"{studentInfo.FamilyName.Trim()}.{studentInfo.GivenName.Trim()}.{dateWithoutSlahes}",
                    studentInfo.FamilyName.Trim(), studentInfo.GivenName.Trim());

                studentInfo.ProfilePicture = $"\\{studentInfo.FamilyName.Trim()}.{studentInfo.GivenName.Trim()}\\{studentInfo.FamilyName.Trim()}." +
                    $"{studentInfo.GivenName.Trim()}.{dateWithoutSlahes}.png";
            }

            var studentsInfoJson = JsonConvert.SerializeObject(studentsInfo, Formatting.Indented);
            await _jsonService.SaveDataAsJsonAsync(studentsInfoJson, "students.json");

            return true;

        }

        public async Task<bool> CreateFoldersForStudentsAsync(List<StudentInfo> studentsInfo)
        {
            try
            {

                foreach (var studentInfo in studentsInfo)
                {
                    string basePath = FileSystem.AppDataDirectory;
                    string studentFolderPath = Path.Combine(basePath, $"{studentInfo.FamilyName.Trim()}.{studentInfo.GivenName.Trim()}");

                    if (!Directory.Exists(studentFolderPath))
                    {
                        Directory.CreateDirectory(studentFolderPath);
                    }

                }
                var studentsInfoJson = JsonConvert.SerializeObject(studentsInfo, Formatting.Indented);
                await _jsonService.SaveDataAsJsonAsync(studentsInfoJson, "students.json");
                await SaveSmartschoolProfilePictureToStudentFolderAsync();

                return true;
            }

            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Er was een probleem met het aanmaken van de mappen {ex.InnerException.Message}", "OK");
                return false;
            }
        }

        public Task<string> LoadImageFromLocalFolder(string imageName)
        {
            if (string.IsNullOrWhiteSpace(imageName))
            {
                return null;
            }
			try
			{

				string[] parts = imageName.Split('.');
				string result = string.Join(".", parts[0], parts[1]);

				string picturesDirectory = FileSystem.AppDataDirectory;
                string albumPath = Path.Combine(picturesDirectory, result);
                string filePath = Path.Combine(albumPath, imageName);

                if (File.Exists(filePath))
                {
                    byte[] imageBytes = File.ReadAllBytes(filePath);
                    string base64Image = Convert.ToBase64String(imageBytes);
					return Task.FromResult(base64Image);
				}

                return null;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", $"Er was een probleem met het ophalen van de foto {ex.InnerException.Message}", "OK");
                return null;
            }
        }

        public async Task<string> SaveImageToLocalFolder(string base64Image, string imageName, string lastName, string firstName)
        {

            try     
            {
                if (base64Image == "23")
                {
                    return null;
                }
                byte[] imageBytes = Convert.FromBase64String(base64Image);

                string filePath = Path.Combine(FileSystem.AppDataDirectory, $"{lastName.Trim()}.{firstName.Trim()}", $"{imageName}.png");
                await File.WriteAllBytesAsync(filePath, imageBytes);

                return filePath;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Er was een probleem met het opslaan van de foto van {lastName} {firstName}: {ex.Message}", "OK");
                return null;
            }
        }

        public Task<bool> SaveImageToAppData(string pictureName, string base64ImageString)
        {

            return Task.FromResult(true);
        }

        public Task<string> SaveImageToLocalFolder(string base64Image, string imageName, StudentInfo studentInfo)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetLatestPictureForStudentAsync(StudentInfo studentInfo)
        {
            throw new NotImplementedException();
        }
    }
}
