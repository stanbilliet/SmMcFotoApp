using Android.Content;
using PicMe.App.Platforms.Android;
using PicMe.Core.Interfaces.Services;
using Android.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Dependency(typeof(OpenAndroidSettingsService))]
namespace PicMe.App.Platforms.Android
{
    public class OpenAndroidSettingsService : IOpenDeviceSettingsService
    {
        public void GoToWiFiSettings()
        {
            Intent intent = new Intent(Settings.ActionWifiSettings);
            intent.AddCategory(Intent.CategoryDefault);
            intent.SetFlags(ActivityFlags.NewTask);
            Platform.CurrentActivity.StartActivity(intent);
        }
    }
}
