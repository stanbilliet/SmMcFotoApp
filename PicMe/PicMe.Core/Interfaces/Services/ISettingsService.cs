using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicMe.Core.Interfaces.Services
{
	public interface ISettingsService
	{
		Task<bool> SaveSettingsAsync(string schoolName, string clientId, string clientSecret, string soapApiKey, 
			string sender ,string backupAccount, bool identification);
	}
}
