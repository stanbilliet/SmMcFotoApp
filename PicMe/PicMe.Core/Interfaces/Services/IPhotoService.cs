using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicMe.Core.Interfaces.Services
{
	public interface IPhotoService
	{
		Task<string> EncodePictureToBase64Async(string profilePicture);
		Task<ImageSource> DecodePictureFromBase64Async(string profilePicture);
	}
}
