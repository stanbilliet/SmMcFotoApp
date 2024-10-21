using PicMe.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicMe.Core.Interfaces.Services
{
	public interface IStudentService
	{
		Task<List<string>>GetAllStudentsAsync();
		Task<List<string>>GetAllClassCodes();
		Task<List<StudentInfo>>GetStudentsByClassCodeAsync(string classCode);

	}
}
