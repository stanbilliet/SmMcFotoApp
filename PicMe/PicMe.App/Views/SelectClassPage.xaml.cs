using PicMe.App.ViewModels;

namespace PicMe.App;

public partial class SelectClassPage : ContentPage
{
    public SelectClassPage(SelectClassViewModel selectClassViewModel)
    {
        InitializeComponent();
        BindingContext = selectClassViewModel;
    }
}