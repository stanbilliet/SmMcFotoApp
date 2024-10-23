using PicMe.App.ViewModels;

namespace PicMe.App
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _mainViewModel;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = _mainViewModel = new MainViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await _mainViewModel.ChangeNameBySchoolName();
        }
    }

}
