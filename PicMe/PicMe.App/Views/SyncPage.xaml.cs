using PicMe.App.ViewModels;

namespace PicMe.App;

public partial class SyncPage : ContentPage
{
    public SyncPage(SyncViewModel syncViewModel)
    {
        InitializeComponent();
        BindingContext = syncViewModel;
    }
}