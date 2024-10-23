using Android.App;
using Android.Content.PM;
using Android.OS;

namespace PicMe.App.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            _ = CheckAndRequestPermissionsAsync();
        }

        public async Task CheckAndRequestPermissionsAsync()
        {
            var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
            var storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if (cameraStatus != PermissionStatus.Granted)
            {
                cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
            }

            if (storageStatus != PermissionStatus.Granted)
            {
                storageStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
            }


        }
    }

}

