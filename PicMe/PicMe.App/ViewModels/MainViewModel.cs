using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicMe.App.Views;


namespace PicMe.App.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        public string name;

        public async Task ChangeNameBySchoolName()
        {
            var schoolName = await SecureStorage.GetAsync("SchoolName");
            Name = $"Hallo {schoolName}!";
        }

        [RelayCommand]
        private async Task About()
        {

            var popup = new AboutPopup();
            var currentPage = Application.Current.MainPage;
            var result = await currentPage.ShowPopupAsync(popup);


        }
        
    }
}
