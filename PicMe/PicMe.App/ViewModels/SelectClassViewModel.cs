using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicMe.Core.Entities;
using PicMe.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PicMe.App.ViewModels
{
    public partial class SelectClassViewModel : BaseViewModel
    {
        private readonly IStudentService _studentService;

        [ObservableProperty]
        private List<SchoolClass> classes;

        [ObservableProperty]
        private string classCode;

        private string searchClass;

        [ObservableProperty]
        private SchoolClass selectedClass;


        [ObservableProperty]
        private ObservableCollection<SchoolClass> filteredClasses;

        public SelectClassViewModel(IStudentService studentService)
        {
            _studentService = studentService;
            InitializeAsync().ConfigureAwait(false);
        }

        private async Task InitializeAsync()
        {
            await LoadClassesAsync();
        }

        public string SearchClass
        {
            get 
            { 
                return searchClass; 
            }
            set
            {
                if (SetProperty(ref searchClass, value))
                {
                    FilterClasses();
                }
            }
        }


        private async Task LoadClassesAsync()
        {
            try
            {
                var uniqueClasses = await _studentService.GetAllClassCodes();
                if (uniqueClasses != null)
                {
                    Classes = uniqueClasses
                        .Select(classCode => new SchoolClass { ClassCode = classCode })
                        .Distinct()
                        .ToList();
                }
                FilterClasses();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Er is een fout opgetreden: {ex.Message}", "OK");
            }
        }

        private void FilterClasses()
        {
            if (string.IsNullOrWhiteSpace(SearchClass))
            {
                FilteredClasses = new ObservableCollection<SchoolClass>(Classes);
            }
            else
            {
                var filtered = Classes
                    .Where(c => c.ClassCode.ToLower().Contains(SearchClass.ToLower()))
                    .ToList();

                FilteredClasses = new ObservableCollection<SchoolClass>(filtered);
            }
        }

        [RelayCommand]
        private async Task ShowInfoAsync()
        {
            await App.Current.MainPage.DisplayAlert("Info", "Eerst moet u een klas Selecteren en dan kliken op de knop volgende" +
                " om verder te gaan.", "OK");
        }

        [RelayCommand]
        private async Task NavigateToSelectedClassPageAsync()
        {
            if (SelectedClass == null)
            {
                await App.Current.MainPage.DisplayAlert("Fout", "Selecteer aub een klas voordat u verder gaat.", "OK");
                return;
            }
            ClassCode = selectedClass.ClassCode;
            await Shell.Current.GoToAsync($"{nameof(SelectedClassPage)}?classCode={classCode}");

            var successToast = CommunityToolkit.Maui.Alerts.Toast.Make($"U wordt doorverwezen naar de geselecteerde klas ({SelectedClass.ClassCode})", ToastDuration.Long, 14);
            await successToast.Show();
        }

        public override void OnLanguageChanged()
        {
            base.OnLanguageChanged();
        }
    }
}
