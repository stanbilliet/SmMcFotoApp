using PicMe.App.Core.Interfaces.Repositories;
using PicMe.App.Core.Interfaces.Services;
using PicMe.App.ViewModels;

namespace PicMe.App
{
    public partial class App : Application
    {
        public App(IPinService pinService, ISoapRepository soapRepository)
        {
            InitializeComponent();

            var appShellViewModel = new AppShellViewModel(pinService, soapRepository);
            MainPage = new AppShell(appShellViewModel);
        }
    }
}
