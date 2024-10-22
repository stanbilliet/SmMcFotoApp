using CommunityToolkit.Maui.Views;

namespace PicMe.App.Views;

public partial class DeleteConfirmPopup : Popup
{
	public DeleteConfirmPopup()
	{
		InitializeComponent();
	}

    private void OnCancelButtonClicked(object sender, EventArgs e)
    {
        Close(false); // Close the popup and return false (not confirmed)
    }

    // When Delete is clicked
    private void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        Close(true); // Close the popup and return true (confirmed)
    }
}