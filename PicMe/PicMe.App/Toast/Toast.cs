using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
