using Android.Content;
using Android.Provider;
using PicMe.App.Core.Interfaces.Services;
using PicMe.App.Platforms.Android;

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
