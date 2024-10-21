using PicMe.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicMe.Core.Interfaces.Services
{
	public interface IJsonService
	{
		Task<bool>SaveDataAsJsonAsync(string studentsInfo, string fileName);
		Task<string> ReadDataFromJsonAsync(string fileName);
		Task<List<StudentInfo>> ExtractStudentInfoAsync(string jsonData);

    }
}
