using Newtonsoft.Json;
using PicMe.Core.Entities;
using PicMe.Core.Interfaces.Repositories;
using PicMe.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicMe.Core.Services
{
    public class StudentService : IStudentService
    {

        private readonly IJsonService _jsonService;
        private readonly ISoapRepository _soapRepository;
        private readonly IOneRosterRepository _oneRosterRepository;
        public StudentService(IJsonService jsonService, ISoapRepository soapRepository, IOneRosterRepository oneRosterRepository)
        {
            _jsonService = jsonService;
            _soapRepository = soapRepository;
            _oneRosterRepository = oneRosterRepository;
        }

        public async Task<List<string>> GetAllClassCodes()
        {
            try
            {
                var enrollmentsJson = await _jsonService.ReadDataFromJsonAsync("students.json");

                var enrollments = JsonConvert.DeserializeObject<List<StudentInfo>>(enrollmentsJson);

                var uniqueClasses = enrollments
                    .Select(e => e.ClassCode)
                    .Distinct()
                    .ToList();

                return uniqueClasses;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Er is een fout opgetreden: {ex.Message}", "OK");
                return null;
            }
        }

        public async Task<List<string>> GetAllStudentsAsync()
        {
            try
            { 
                var studentsInfoJson = await _jsonService.ReadDataFromJsonAsync("students.json");

                var studentsInfo = JsonConvert.DeserializeObject<List<StudentInfo>>(studentsInfoJson);

                var students = studentsInfo
                    .Select(studentInfo => $"{studentInfo.GivenName} {studentInfo.FamilyName}")
                    .Distinct()
                    .ToList();

                return students;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Er is een fout opgetreden: {ex.Message}", "OK");
                return null;
            }
        }

        public async Task<List<StudentInfo>> GetAllStudentInfo()
        {
            try
            {
                var studentsInfoJson = await _jsonService.ReadDataFromJsonAsync("students.json");
                var students = JsonConvert.DeserializeObject<List<StudentInfo>>(studentsInfoJson);

                return students;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Er is een fout opgetreden: {ex.Message}", "OK");
                return null;
            }
        }

        public async Task<List<StudentInfo>> GetStudentsByClassCodeAsync(string classCode)
        {
            try
            {
                var studentsInfoJson = await _jsonService.ReadDataFromJsonAsync("students.json");
                var students = JsonConvert.DeserializeObject<List<StudentInfo>>(studentsInfoJson);

                var filteredStudents = students
                    .Where(studentInfo => studentInfo.ClassCode.Equals(classCode, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(studentInfo => studentInfo.GivenName)
                    .ToList();

                return filteredStudents;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Er is een fout opgetreden: {ex.Message}", "OK");
                return null;
            }
        }
    }
}
