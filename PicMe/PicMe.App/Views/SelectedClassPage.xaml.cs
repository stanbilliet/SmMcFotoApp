using PicMe.App.ViewModels;
using System.Timers;

namespace PicMe.App;

public partial class SelectedClassPage : ContentPage
{
    public SelectedClassPage(SelectedClassViewModel selectedClassViewModel)
	{
		InitializeComponent();

		BindingContext = selectedClassViewModel;
    }

    protected override bool OnBackButtonPressed()
    {
        var viewModel = (SelectedClassViewModel)BindingContext;

        viewModel.HandleBackButtonPressAsync().ConfigureAwait(false);

        return true;
    }
}