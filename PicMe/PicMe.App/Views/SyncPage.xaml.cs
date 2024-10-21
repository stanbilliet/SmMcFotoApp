using PicMe.App.ViewModels;
using PicMe.Core.Interfaces.Repositories;

namespace PicMe.App;

public partial class SyncPage : ContentPage
{
    public SyncPage(SyncViewModel syncViewModel)
	{
	    InitializeComponent();
	    BindingContext = syncViewModel;
	}
}