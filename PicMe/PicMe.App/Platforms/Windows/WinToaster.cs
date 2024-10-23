using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace PicMe.App.Platforms.Windows
{
    public static class WinToaster
    {
        public static async Task ToastAlertAsync(string message)
        {

            //show alert

            await App.Current.MainPage.DisplayAlert("Allert", $"{message}", "OK");


        }
    }
}
