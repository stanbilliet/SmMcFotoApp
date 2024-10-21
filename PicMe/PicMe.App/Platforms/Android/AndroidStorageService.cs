using PicMe.Core.Entities;
using PicMe.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Android.OS;
using Newtonsoft.Json;
using PicMe.Core.Interfaces.Repositories;
using AndroidX.Core.App;
using Android.App;
using Android.Content;
using Android.Provider;
using Application = Android.App.Application;
using PicMe.Platforms.Android;
using PicMe.App;
using PicMe.App.Toast;
using Environment = Android.OS.Environment;
using PicMe.Core.Helpers;

[assembly: Dependency(typeof(AndroidStorageService))]
namespace PicMe.Platforms.Android
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

		public async Task<bool> SaveSmartschoolProfilePictureToStudentFolderAsync()
		{
			var studentsData = await _jsonService.ReadDataFromJsonAsync("students.json");
			var studentsInfo = JsonConvert.DeserializeObject<List<StudentInfo>>(studentsData);

			foreach (var studentInfo in studentsInfo)
			{
				var base64picture = await _soapRepository.GetBase64ProfilePictureAsync(studentInfo.Identifier);

				string imageName = $"{Guid.NewGuid()}";
				await SaveImageToLocalFolder(base64picture, imageName, studentInfo);
				studentInfo.ProfilePicture = string.Empty;
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

					string basePath = Path.Combine(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures).AbsolutePath, "PicMe");
					string studentFolderPath = Path.Combine(basePath, $"{studentInfo.Identifier.Trim()}");

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
				await Toast.ToastAlertAsync($"Er was een probleem met het aanmaken van de mappen {ex.Message}");
				return false;
			}
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

        public async Task<bool> SaveImageToAppData(string pictureName, string base64ImageString)
		{
			try
			{
				string documentsPath = FileSystem.AppDataDirectory;
				string oldFilePath = $"{documentsPath}/{pictureName}";
				string tempFilePath = $"{documentsPath}/{pictureName}.temp";

				if (File.Exists(tempFilePath))
				{
					File.Delete(tempFilePath);
				}

				byte[] imageBytes = Convert.FromBase64String(base64ImageString);
				File.WriteAllBytes(tempFilePath, imageBytes);

				if (File.Exists(oldFilePath))
				{
					File.Delete(oldFilePath);
				}

				File.Move(tempFilePath, oldFilePath);

				return true;
			}
			catch (Exception ex)
			{
				await Toast.ToastAlertAsync($"Er ging iets mis bij het opslaan van het bestand van {pictureName} {ex.Message} OK");
				return false;
			}
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
                // Display a toast message if there's an error
                await Toast.ToastAlertAsync($"Er was een probleem met het opslaan van de foto van {studentInfo.FamilyName} {studentInfo.GivenName}: {ex.Message}");
                return null;
            }

		}

		public Task<string> LoadImageFromLocalFolder(string pictureName)
		{

			string documentsPath = FileSystem.AppDataDirectory;
			string filePath = Path.Combine(documentsPath, pictureName);

			if (File.Exists(filePath))
			{
				string base64Image = File.ReadAllText(filePath);
				return Task.FromResult(base64Image);
			}
			return null;
		}
    }
}