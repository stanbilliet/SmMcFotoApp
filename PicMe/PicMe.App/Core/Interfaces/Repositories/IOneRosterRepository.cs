using PicMe.App.Core.Entities;

namespace PicMe.App.Core.Interfaces.Repositories
{
    public interface IOneRosterRepository
    {
        Task<string> GetAccessTokenAsync();
        Task<List<StudentInfo>> GetAllEnrollmentsAsync();

    }
}
