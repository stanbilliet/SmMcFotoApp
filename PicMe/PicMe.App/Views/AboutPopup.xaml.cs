using CommunityToolkit.Maui.Views;

namespace PicMe.App.Views;

public partial class AboutPopup : Popup
{
    public AboutPopup()
    {
        InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close(true);
    }
}