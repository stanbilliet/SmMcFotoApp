using PicMe.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicMe.Core.Services
{
	public  class SecureStorageService : ISecureStorageService
	{
		public async Task<string> GetAsync(string key)
		{
			return await SecureStorage.Default.GetAsync(key);
		}

		public async Task SetAsync(string key, string value)
		{
			await SecureStorage.Default.SetAsync(key, value);
		}
	}
}
