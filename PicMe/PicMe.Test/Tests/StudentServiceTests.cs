using Moq;
using Newtonsoft.Json;
using PicMe.Core.Entities;
using PicMe.Core.Interfaces.Repositories;
using PicMe.Core.Interfaces.Services;
using PicMe.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicMe.Test.Tests
{
    public class StudentServiceTests
    {
        private readonly Mock<IJsonService> _jsonServiceMock;
        private readonly Mock<ISoapRepository> _soapRepositoryMock;
        private readonly Mock<IOneRosterRepository> _oneRosterRepositoryMock;
        private readonly StudentService _studentService;

        public StudentServiceTests()
        {
            _jsonServiceMock = new Mock<IJsonService>();
            _soapRepositoryMock = new Mock<ISoapRepository>();
            _oneRosterRepositoryMock = new Mock<IOneRosterRepository>();

            _studentService = new StudentService(_jsonServiceMock.Object, _soapRepositoryMock.Object, _oneRosterRepositoryMock.Object);
        }
        //[Fact]
        //public async Task GetAllClassCodes_ValidData_ReturnsUniqueClasses()
        //{
        //    // Arrange
        //    var enrollmentsJson = JsonConvert.SerializeObject(new List<StudentInfo>
        //    {
        //        new() { ClassCode = "ClassA" },
        //        new() { ClassCode = "ClassB" },
        //        new() { ClassCode = "ClassA" }
        //    });

        //    _jsonServiceMock.Setup(js => js.ReadDataFromJsonAsync("students.json")).ReturnsAsync(enrollmentsJson);

        //    // Act
        //    var result = await _studentService.GetAllClassCodes();

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(2, result.Enrollments.Count);
        //    Assert.Contains(result.Enrollments, e => e.SchoolClasses.ClassCode == "ClassA");
        //    Assert.Contains(result.Enrollments, e => e.SchoolClasses.ClassCode == "ClassB");
        //}
        //[Fact]
        //public async Task GetAllClassCodes_EmptyData_ReturnsEmptyRoot()
        //{
        //    // Arrange
        //    _jsonServiceMock.Setup(js => js.ReadDataFromJsonAsync("students.json")).ReturnsAsync("[]");

        //    // Act
        //    var result = await _studentService.GetAllClassCodes();

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Empty(result.Enrollments);
        //}
        [Fact]
        public async Task GetAllClassCodes_ExceptionThrown_ReturnsNull()
        {
            //Arrange
            _jsonServiceMock.Setup(js => js.ReadDataFromJsonAsync("students.json")).ThrowsAsync(new Exception());

            // Act
            var result = await _studentService.GetAllClassCodes();

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task GetAllStudentsAsync_ValidData_ReturnsStudents()
        {
            // Arrange
            var studentsJson = JsonConvert.SerializeObject(new List<StudentInfo>
            {
                new() { GivenName = "John", FamilyName = "Doe" },
                new() { GivenName = "Jane", FamilyName = "Smith" }
            });

            _jsonServiceMock.Setup(js => js.ReadDataFromJsonAsync("students.json")).ReturnsAsync(studentsJson);

            // Act
            var result = await _studentService.GetAllStudentsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Enrollments.Count);
            Assert.Contains(result.Enrollments, e => e.Students.GivenName == "John" && e.Students.FamilyName == "Doe");
            Assert.Contains(result.Enrollments, e => e.Students.GivenName == "Jane" && e.Students.FamilyName == "Smith");
        }
        [Fact]
        public async Task GetAllStudentsAsync_EmptyData_ReturnsEmptyRoot()
        {
            // Arrange
            _jsonServiceMock.Setup(js => js.ReadDataFromJsonAsync("students.json")).ReturnsAsync("[]");

            // Act
            var result = await _studentService.GetAllStudentsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Enrollments);
        }
        [Fact]
        public async Task GetAllStudentsAsync_ExceptionThrown_ReturnsNull()
        {
            // Arrange
            _jsonServiceMock.Setup(js => js.ReadDataFromJsonAsync("students.json")).ThrowsAsync(new Exception());

            // Act
            var result = await _studentService.GetAllStudentsAsync();

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task GetStudentsByClassCodeAsync_ValidClassCode_ReturnsFilteredStudents()
        {
            // Arrange
            var studentsJson = JsonConvert.SerializeObject(new List<StudentInfo>
            {
                new() { ClassCode = "ClassA", GivenName = "John", FamilyName = "Doe" },
                new() { ClassCode = "ClassB", GivenName = "Jane", FamilyName = "Smith" }
            });

            _jsonServiceMock.Setup(js => js.ReadDataFromJsonAsync("students.json")).ReturnsAsync(studentsJson);

            // Act
            var result = await _studentService.GetStudentsByClassCodeAsync("ClassA");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Enrollments);

            var enrollment = result.Enrollments.ElementAt(0);
            Assert.Equal("John", enrollment.Students.GivenName);
            Assert.Equal("Doe", enrollment.Students.FamilyName);
            Assert.Equal("ClassA", enrollment.SchoolClasses.ClassCode);
        }

        [Fact]
        public async Task GetStudentsByClassCodeAsync_NoMatchingClassCode_ReturnsEmptyRoot()
        {
            // Arrange
            var studentsJson = JsonConvert.SerializeObject(new List<StudentInfo>
            {
                new() { ClassCode = "ClassA", GivenName = "John", FamilyName = "Doe" }
            });

            _jsonServiceMock.Setup(js => js.ReadDataFromJsonAsync("students.json")).ReturnsAsync(studentsJson);

            // Act
            var result = await _studentService.GetStudentsByClassCodeAsync("ClassB");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Enrollments);
        }
        [Fact]
        public async Task GetStudentsByClassCodeAsync_ExceptionThrown_ReturnsNull()
        {
            // Arrange
            _jsonServiceMock.Setup(js => js.ReadDataFromJsonAsync("students.json")).ThrowsAsync(new Exception());

            // Act
            var result = await _studentService.GetStudentsByClassCodeAsync("ClassA");

            // Assert
            Assert.Null(result);
        }
    }
}
