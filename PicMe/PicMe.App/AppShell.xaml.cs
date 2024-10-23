using PicMe.App.ViewModels;
using PicMe.App.Views;
using System.Timers;

namespace PicMe.App
{
    public partial class AppShell : Shell
    {
        private int backButtonPressCount = 0;
        private const int maxBackPressCount = 2;
        private System.Timers.Timer resetTimer;

        public AppShell(AppShellViewModel appShellViewModel)
        {
            InitializeComponent();
            BindingContext = appShellViewModel;

            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(SelectClassPage), typeof(SelectClassPage));
            Routing.RegisterRoute(nameof(SelectedClassPage), typeof(SelectedClassPage));
            Routing.RegisterRoute(nameof(SyncPage), typeof(SyncPage));
            Routing.RegisterRoute(nameof(PinCodeModalPage), typeof(PinCodeModalPage));

            resetTimer = new System.Timers.Timer(2000);
            resetTimer.Elapsed += OnResetTimerElapsed;
            resetTimer.AutoReset = false;

        }

        protected override bool OnBackButtonPressed()
        {
            backButtonPressCount++;
            if (backButtonPressCount == 1)
            {
                Toast.ToastShowter.ToastAlertAsync("Druk nog een keer om terug te keren.");

                resetTimer.Start();
            }
            else if (backButtonPressCount >= maxBackPressCount)
            {
                resetTimer.Stop();
                backButtonPressCount = 0;
                return base.OnBackButtonPressed();
            }

            return true;
        }

        private void OnResetTimerElapsed(object sender, ElapsedEventArgs e)
        {
            backButtonPressCount = 0;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            backButtonPressCount = 0;
            resetTimer.Stop();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            resetTimer.Stop();
        }
    }
}
