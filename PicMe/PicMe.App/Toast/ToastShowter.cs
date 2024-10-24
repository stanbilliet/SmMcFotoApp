using CommunityToolkit.Maui.Core;


namespace PicMe.App.Toast
{
    public static class ToastShowter
    {
        public static async Task ToastAlertAsync(string message)
        {

//#if WINDOWS
//           await PicMe.App.Platforms.Windows.WinToaster.ToastAlertAsync(message);
//#else
            CancellationTokenSource cancellationTokenSource = new();

            var toast = CommunityToolkit.Maui.Alerts.Toast.Make(message, ToastDuration.Short, 14);
            await toast.Show(cancellationTokenSource.Token);

//#endif


        }
    }
}
