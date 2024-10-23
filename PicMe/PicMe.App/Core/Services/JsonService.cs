using Newtonsoft.Json;
using PicMe.App.Core.Entities;
using PicMe.App.Core.Interfaces.Repositories;
using PicMe.App.Core.Interfaces.Services;
using CommunityToolkit.Maui.Alerts;


namespace PicMe.App.Core.Services
{
    public class JsonService : IJsonService
    {
        private readonly ISoapRepository _soapRepository;

        public JsonService(ISoapRepository soapRepository)
        {
            _soapRepository = soapRepository;
        }


        public async Task<string> ReadDataFromJsonAsync(string fileName)
        {
            try
            {
                string documentsPath = FileSystem.AppDataDirectory;
                string filePath = Path.Combine(documentsPath, fileName);

                if (File.Exists(filePath))
                {
                    string jsonData = await File.ReadAllTextAsync(filePath);
                    return jsonData;
                }
                else
                {
                  var toast =  CommunityToolkit.Maui.Alerts.Toast.Make("Er is geen bestand gevonden op deze locatie, moet je nog synchroniseren?");

                    await toast.Show();

                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"er is iets misgelopen bij het uitlezen van het bestand", "OK");
                return string.Empty;
            }
        }

        public async Task<bool> SaveDataAsJsonAsync(string studentsInfoJson, string fileName)
        {
            try
            {
                string documentsPath = FileSystem.AppDataDirectory;
                string oldFilePath = Path.Combine(documentsPath, fileName);
                string tempFilePath = Path.Combine(documentsPath, fileName + ".temp");

                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }

                await File.WriteAllTextAsync(tempFilePath, studentsInfoJson);

                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }

                File.Move(tempFilePath, oldFilePath);

                return true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"er is iets misgelopen bij het opslaan van het bestand " +
                    $"{ex.Message}", "OK");
                return false;
            }
        }

        public async Task<List<StudentInfo>> ExtractStudentInfoAsync(string jsonData)
        {

            var root = JsonConvert.DeserializeObject<Root>(jsonData);

            var studentInfos = root.Enrollments.Select(enrollment => new StudentInfo
            {
                Identifier = enrollment.Students.Identifier,
                GivenName = enrollment.Students.GivenName,
                FamilyName = enrollment.Students.FamilyName,
                InternalNumber = enrollment.Students.MetaData.InternalNumber,
                ClassCode = enrollment.SchoolClasses.ClassCode
            }).ToList();

            return await Task.FromResult(studentInfos);
        }
    }
}
