using PicMe.App.ViewModels;
using PicMe.Core.Interfaces.Repositories;
using PicMe.Core.Interfaces.Services;

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
