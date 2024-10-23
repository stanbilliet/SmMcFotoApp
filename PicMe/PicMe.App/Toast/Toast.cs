using CommunityToolkit.Maui.Core;

namespace PicMe.App.Toast
{
    public static class Toast
    {
        public static async Task ToastAlertAsync(string message)
        {
            CancellationTokenSource cancellationTokenSource = new();

            var toast = CommunityToolkit.Maui.Alerts.Toast.Make(message, ToastDuration.Short, 14);
            await toast.Show(cancellationTokenSource.Token);
        }
    }
}
