using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using System.Threading;
using Font = Microsoft.Maui.Font;

namespace PicMe.App.Platforms.Windows
{
    public static class WinToaster
    {
        public static async Task ToastAlertAsync(string message)
        {

            await App.Current.MainPage.DisplayAlert("Allert", $"{message}", "OK");


        }
    }
}
