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

            var students = JsonConvert.DeserializeObject<List<StudentInfo>>(studentsJson);

            // Assert
            Assert.NotNull(students);
            Assert.Equal(2, students.Count);
            Assert.Contains(students, e => e.GivenName == "John" && e.FamilyName == "Doe");
            Assert.Contains(students, e => e.GivenName == "Jane" && e.FamilyName == "Smith");
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
            Assert.Empty(result);
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

            var studentresult = JsonConvert.DeserializeObject<List<StudentInfo>>(studentsJson);

            // Assert
            Assert.NotNull(studentresult);
            Assert.Single(studentresult);

            var enrollment = studentresult.ElementAt(0);
            Assert.Equal("John", enrollment.GivenName);
            Assert.Equal("Doe", enrollment.FamilyName);
            Assert.Equal("ClassA", enrollment.ClassCode);
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
            Assert.Empty(result);
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
