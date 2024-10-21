using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicMe.Core.Interfaces.Repositories;
using PicMe.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.Json;
using Newtonsoft.Json;

namespace PicMe.App.ViewModels
{
    public partial class SyncViewModel(IOneRosterRepository oneRosterRepository, IJsonService jsonService) : BaseViewModel
    {

        private readonly IOneRosterRepository _oneRosterRepository = oneRosterRepository;
        private readonly IJsonService _jsonService = jsonService;

        [ObservableProperty]
        private bool _isBusy;

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

                //var enrollmentsJson = System.Text.Json.JsonSerializer.Serialize(enrollments);

                if (enrollments)
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

        public override void OnLanguageChanged()
        {
            base.OnLanguageChanged();
        }
    }
}
