using PicMe.App.Core.Entities;

namespace PicMe.App.Core.Interfaces.Services
{
    public interface IJsonService
    {
        Task<bool> SaveDataAsJsonAsync(string studentsInfo, string fileName);
        Task<string> ReadDataFromJsonAsync(string fileName);
        Task<List<StudentInfo>> ExtractStudentInfoAsync(string jsonData);

    }
}
