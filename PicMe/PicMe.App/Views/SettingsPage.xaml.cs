using PicMe.App.ViewModels;

namespace PicMe.App;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel _settingsViewModel;

    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _settingsViewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _settingsViewModel.LoadSettingsAsync();
    }

    private void OnEntryCompleted(object sender, EventArgs e)
    {
        var currentEntry = sender as Entry;

        if (currentEntry == null)
            return;

        switch (currentEntry)
        {
            case var entry when entry == schoolEntry:
                clientIdEntry.Focus();
                break;
            case var entry when entry == clientIdEntry:
                clientSecretEntry.Focus();
                break;
            case var entry when entry == clientSecretEntry:
                apiKeyEntry.Focus();
                break;
            case var entry when entry == apiKeyEntry:
                senderEntry.Focus();
                break;
            case var entry when entry == senderEntry:
                backupEntry.Focus();
                break;
        }

    }
}