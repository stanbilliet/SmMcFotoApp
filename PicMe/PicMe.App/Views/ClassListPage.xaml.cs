using PicMe.App.ViewModels;

namespace PicMe.App;

public partial class ClassListPage : ContentPage
{
	public ClassListPage(ClassListViewModel classListViewModel)
	{
		InitializeComponent();
		BindingContext = classListViewModel;

    }
}