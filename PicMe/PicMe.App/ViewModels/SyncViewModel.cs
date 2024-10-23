using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicMe.App.Core.Interfaces.Repositories;
using PicMe.App.Core.Interfaces.Services;
using PicMe.App.Views;

namespace PicMe.App.ViewModels
{
    public partial class SyncViewModel(IOneRosterRepository oneRosterRepository, IJsonService jsonService, IStorageService storageService) : BaseViewModel
    {

        private readonly IOneRosterRepository _oneRosterRepository = oneRosterRepository;
        private readonly IJsonService _jsonService = jsonService;
        private readonly IStorageService _storageService = storageService;

        [ObservableProperty]
        private double _progressValue;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private bool _syncPicturesFromSmSc;

        [RelayCommand]
        private async Task SyncData()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                await Toast.Toast.ToastAlertAsync("Synchroniseren is begonnen! Dit kan even duren! Even geduld aub!");

                var enrollments = await _oneRosterRepository.GetAllEnrollmentsAsync();

                var localProgress = 1.0 / enrollments.Count;

                var result = await _storageService.CreateStudentJsonFile(enrollments);

                foreach (var enrollment in enrollments)
                {

                    await _storageService.CreateFoldersForStudentsAsync(enrollment);
                    if (SyncPicturesFromSmSc)
                    {
                        await _storageService.SaveSmartschoolProfilePictureToStudentFolderAsync(enrollment);
                    }

                    ProgressValue += localProgress;

                }

                ProgressValue = 0;


                if (result)
                {
                    await Application.Current.MainPage.DisplayAlert("Succes", "Data is gesynchroniseerd en opgeslagen!", "Ok");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Fout", "Data kon niet opgeslagen worden.", "Ok");
                }



            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", $"Er is een probleem opgetreden: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ClearData()
        {
            var popup = new DeleteConfirmPopup();
            var currentPage = Application.Current.MainPage;
            var result = await currentPage.ShowPopupAsync(popup);

            if ((bool)result)
            {
                await _storageService.DeleteStudentPictures();

                await Toast.Toast.ToastAlertAsync("Data is verwijderd!");

            }
        }

        public override void OnLanguageChanged()
        {
            base.OnLanguageChanged();
        }
    }
}
