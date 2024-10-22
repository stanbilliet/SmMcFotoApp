using PicMe.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Metrics;
using System.Text;
using System.Threading.Tasks;

namespace PicMe.Core.Interfaces.Repositories
{
	public interface IOneRosterRepository
	{
		Task<string> GetAccessTokenAsync();
		Task<List<StudentInfo>> GetAllEnrollmentsAsync();
		
	}
}
