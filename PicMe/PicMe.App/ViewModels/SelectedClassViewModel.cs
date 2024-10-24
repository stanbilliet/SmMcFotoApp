﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicMe.Core.Entities;
using PicMe.Core.Interfaces.Repositories;
using PicMe.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace PicMe.App.ViewModels
{
    [QueryProperty(nameof(ClassCode), "classCode")]
    public partial class SelectedClassViewModel : ObservableObject
    {
        private readonly IStudentService _studentService;
        private readonly IStorageService _storageService;
        private readonly ISoapRepository _soapRepository;

        private int backButtonPressCount = 0;
        private const int maxBackPressCount = 2;
        private System.Timers.Timer resetTimer;

        [ObservableProperty]
        private string classCode;

        [ObservableProperty]
        private ObservableCollection<StudentInfo> studentsInfo;

        private bool hasUpdatedStudents;
        public bool HasUpdatedStudents
        {
            get
            {
                return StudentsInfo != null && StudentsInfo.Any(s => s.IsUpdated);
            }
        }

        public SelectedClassViewModel(IStudentService studentService, IStorageService storageService,
            ISoapRepository soapRepository)
        {
            _studentService = studentService;
            _storageService = storageService;
            _soapRepository = soapRepository;

            resetTimer = new System.Timers.Timer(2000);
            resetTimer.Elapsed += OnResetTimerElapsed;
            resetTimer.AutoReset = false;
        }

        partial void OnClassCodeChanged(string value)
        {
            LoadStudentsAsync(value).ConfigureAwait(false);
        }

        private async Task LoadStudentsAsync(string classCode)
        {
            try
            {
                var students = await _studentService.GetStudentsByClassCodeAsync(classCode);
                if (students != null)
                {
                    var studentInfoList = new List<StudentInfo>();
                    foreach (var student in students)
                    {
                        string filepath = FileSystem.AppDataDirectory;
#if WINDOWS
                        student.ImagePath = filepath + student.ProfilePicture;
#elif ANDROID
						//student.ImagePath = $"{filepath}/{student.ProfilePicture}";
                        student.ImagePath = await _storageService.GetLatestPictureForStudentAsync(student);
#endif
                        studentInfoList.Add(student);
                    }

                    StudentsInfo = new ObservableCollection<StudentInfo>(studentInfoList);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Er zijn geen leerlingen in deze klas ({classCode})", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Er is een fout opgetreden: {ex.Message}", "OK");
            }
        }


        [RelayCommand]
        private async Task TakeNewPictureAsync(StudentInfo studentInfo)
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();

                if (photo != null)
                {
                    using (var stream = await photo.OpenReadAsync())
                    {
                        byte[] photoBytes;
                        using (var memoryStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            photoBytes = memoryStream.ToArray();
                        }

                        string base64Image = Convert.ToBase64String(photoBytes);
                        string date = DateTime.Now.ToShortDateString();
                        string dateWithoutSlahes = date.Replace("/", "");
                        if (string.IsNullOrWhiteSpace(studentInfo.ProfilePicture))
                        {
                            return;
                        }

                        //bool isSavedToAppData = await _storageService.SaveImageToAppData(
                        //    studentInfo.ProfilePicture,
                        //    base64Image
                        //);

                        string savedImageUri = await _storageService.SaveImageToLocalFolder(
                            base64Image,
                            $"{studentInfo.Identifier.Trim()}{dateWithoutSlahes}",
                            studentInfo);

                        if (!string.IsNullOrWhiteSpace(savedImageUri))
                        {


                            var student = StudentsInfo.FirstOrDefault(s => s.Identifier == studentInfo.Identifier);


                            if (student != null)
                            {
                                var index = StudentsInfo.IndexOf(student);
                                student.IsUpdated = true;
                                //student.ProfilePicture = savedImageUri;
                                student.ImagePath = savedImageUri;
                                student.ProfilePicture = base64Image;

                                StudentsInfo[index] = student;

                                OnPropertyChanged(nameof(HasUpdatedStudents));
                            }

                            await Application.Current.MainPage.DisplayAlert("Succes", $"Foto van {studentInfo.GivenName} " +
                                $"{studentInfo.FamilyName} is succesvol genomen en opgeslagen.", "OK");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error",
                                $"Er is een fout opgetreden bij het opslaan van de foto van " +
                                $"{studentInfo.GivenName} {studentInfo.FamilyName}.", "OK");
                        }
                    }
                }
            }
            catch (FeatureNotSupportedException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Camera niet beschikbaar op dit apparaat: {ex.Message}", "OK");
            }
            catch (PermissionException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Geen toestemming om de camera te gebruiken: {ex.Message}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Er is een fout opgetreden: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task SetStudentAccountPictureAsync(StudentInfo studentInfo)
        {
            try
            {
                if (studentInfo.IsUpdated)
                {


                    string base64Image = studentInfo.ProfilePicture;

                    string result = await _soapRepository.SetAccountPhotoAsync(base64Image,
                        studentInfo.Identifier);

                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        await Toast.Toast.ToastAlertAsync($"Foto van {studentInfo.GivenName} " +
                                                           $"{studentInfo.FamilyName} is succesvol geupdate.");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error",
                            $"Er is een fout opgetreden bij het updaten van de foto van " +
                            $"{studentInfo.GivenName} {studentInfo.FamilyName}.", "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error",
                                                   $"De foto van {studentInfo.GivenName} {studentInfo.FamilyName} " +
                                                   $"is niet gevonden.", "OK");
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Er is een fout opgetreden: {ex.Message}", "OK");
            }
            finally
            {
                if (studentInfo.IsUpdated)
                {
                    studentInfo.IsUpdated = false;
                    OnPropertyChanged(nameof(HasUpdatedStudents));
                }
            }
        }

        [RelayCommand]
        private async Task SetStudentsAccountPictureAsync()
        {
            try
            {
                var updatedStudents = StudentsInfo.Where(s => s.IsUpdated).ToList();

                if (updatedStudents.Any())
                {
                    foreach (var student in updatedStudents)
                    {
                        await SetStudentAccountPictureAsync(student);
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Info", "Geen geupdated foto's om te verzenden.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Er is een fout opgetreden: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task BackToSelectClassPageAsync()
        {
            if (HasUpdatedStudents)
            {
                var updatedStudentsCount = StudentsInfo.Count(s => s.IsUpdated);
                var confirm = await Application.Current.MainPage.DisplayAlert("Waarschuwing",
                    $"Er zijn {updatedStudentsCount} studenten met een ongeupdate foto. " +
                    "Weet je zeker dat je wilt terugkeren?", "Ja", "Nee");

                if (confirm)
                {
                    resetTimer.Stop();
                    backButtonPressCount = 0;
                    await Shell.Current.GoToAsync($"//{nameof(SelectClassPage)}");
                }
                else
                {
                    backButtonPressCount = 0;
                    resetTimer.Stop();
                }
            }
            else
            {
                resetTimer.Stop();
                backButtonPressCount = 0;
                await Shell.Current.GoToAsync($"//{nameof(SelectClassPage)}");
            }
        }

        public async Task HandleBackButtonPressAsync()
        {
            if (HasUpdatedStudents)
            {
                var updatedStudentsCount = StudentsInfo.Count(s => s.IsUpdated);
                var confirm = await Application.Current.MainPage.DisplayAlert("Waarschuwing",
                    $"Er zijn {updatedStudentsCount} studenten met een ongeupdate foto. " +
                    "Weet je zeker dat je wilt terugkeren?", "Ja", "Nee");

                if (confirm)
                {
                    resetTimer.Stop();
                    backButtonPressCount = 0;
                    await Shell.Current.GoToAsync($"//{nameof(SelectClassPage)}");
                }
                else
                {
                    backButtonPressCount = 0;
                    resetTimer.Stop();
                }
            }
            else
            {
                resetTimer.Stop();
                backButtonPressCount = 0;
                await Shell.Current.GoToAsync($"//{nameof(SelectClassPage)}");
            }
        }

        private void OnResetTimerElapsed(object sender, ElapsedEventArgs e)
        {
            backButtonPressCount = 0;
        }
    }
}
