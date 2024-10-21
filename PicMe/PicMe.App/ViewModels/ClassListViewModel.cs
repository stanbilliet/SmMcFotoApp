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
    public partial class ClassListViewModel : ObservableObject
    {
        private readonly IStudentService _studentService;

        [ObservableProperty]
        private List<SchoolClass> classes;

        [ObservableProperty]
        private string classCode;

        [ObservableProperty]
        private SchoolClass selectedClass;

        [ObservableProperty]
        private string searchClass;

        [ObservableProperty]
        private ObservableCollection<SchoolClass> filteredClasses;

        [ObservableProperty]
        private string currentClassImage;


        public string StandardImage { get; set; } = "Resources/images/standard_class_image.png";

        public ClassListViewModel(IStudentService studentService)
        {
            _studentService = studentService;

            currentClassImage = StandardImage;

            InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await LoadClassesAsync();
        }

        private async Task LoadClassesAsync()
        {
            try
            {
                var uniqueClasses = await _studentService.GetAllClassCodes();
                if(uniqueClasses != null)
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
                await Application.Current.MainPage.DisplayAlert("Error", $"Er is een fout opgetreden: {ex.Message}", "OK");
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
        private async Task OnUploadPictureAsync(SchoolClass schoolClass)
        {

            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Selecteer een afbeelding om te uploaden",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, result.FileName);

                    using Stream sourceStream = await result.OpenReadAsync();
                    using FileStream localFileStream = File.OpenWrite(localFilePath);

                    await sourceStream.CopyToAsync(localFileStream);

                    schoolClass.ClassProfilePicture = localFilePath;

                    await Toast.Toast.ToastAlertAsync($"Afbeelding is geüpload voor klas ({ClassCode})");
                }
                else
                {
                    await Toast.Toast.ToastAlertAsync("Geen afbeelding geselecteerd.");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fout",
                    $"Er ging iets mis tijdens het uploaden: {ex.Message}",
                    "OK");
            }
        }

        
        [RelayCommand]
        private async Task OnTakeNewPictureAsync(SchoolClass schoolClass)
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                    if (photo != null)
                    {
                        string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                        using Stream sourceStream = await photo.OpenReadAsync();
                        using FileStream localFileStream = File.OpenWrite(localFilePath);

                        await sourceStream.CopyToAsync(localFileStream);

                        await Toast.Toast.ToastAlertAsync($"Afbeelding is gemaakt voor klas ({ClassCode})");

                        schoolClass.ClassProfilePicture = localFilePath;
                        await Toast.Toast.ToastAlertAsync($"Afbeelding is geüpload voor klas ({schoolClass.ClassCode})");
                    }
                    else
                    {
                        await Toast.Toast.ToastAlertAsync("Actie geannuleerd. Geen foto gemaakt.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fout",
                $"Er ging iets mis tijdens het nemen van een nieuwe foto: {ex.Message}", "OK");
                return;
            }
        }

        [RelayCommand]
        private async Task ShowInfoAsync()
        {
            await App.Current.MainPage.DisplayAlert("Informatie",
                "Druk eerst op een klas om deze te markeren.\n\n" +
                "Daarna kun je één van de drie acties uitvoeren via de knoppen:\n\n" +
                "**Camera (📷):** Maak een foto die direct in de app wordt opgeslagen en niet kan worden aangepast.\n\n" +
                "**Uploaden (⬆️):** Neem een foto met de camera-app van je telefoon, bewerk deze indien nodig en upload naar de app.\n\n" +
                "**Verzenden (➡️):** Stuur de geselecteerde afbeelding naar de opgegeven bestemming.",
                "OK");
        }

        [RelayCommand]
        private async Task SendPictureToClassStudentsAsync()
        {
            await Toast.Toast.ToastAlertAsync("Foto is verstuurd naar alle leerlingen van de klas.");
        }
    }
}
