using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicMe.Core.Interfaces.Services
{
	public interface IPinService
	{
		Task<string> SetNewPinAsync();
	}
}
