using PicMe.App.Core.Entities;

namespace PicMe.App.Core.Interfaces.Services
{
    public interface IStudentService
    {
        Task<List<string>> GetAllStudentsAsync();
        Task<List<string>> GetAllClassCodes();
        Task<List<StudentInfo>> GetStudentsByClassCodeAsync(string classCode);
        Task<List<StudentInfo>> GetAllStudentInfo();


    }
}
