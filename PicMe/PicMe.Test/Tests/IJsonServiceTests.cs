using Moq;
using PicMe.Core.Interfaces.Repositories;
using PicMe.Core.Interfaces.Services;
using PicMe.Core.Services;
using PicMe.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace PicMe.Test.Tests
{
    public class JsonServiceTests
    {
        private Mock<ISoapRepository> _soapRepositoryMock;
        private IJsonService _jsonService;

        public JsonServiceTests()
        {
            _soapRepositoryMock = new Mock<ISoapRepository>();
            _jsonService = new JsonService(_soapRepositoryMock.Object);
        }

        [Fact]
        public async Task ExtractStudentInfoAsync_ValidJsonData_ReturnsStudentInfoList()
        {
            // Arrange
            string jsonData = @"
            {
                ""enrollments"": [
                    {
                        ""user"": {
                            ""identifier"": ""123"",
                            ""givenName"": ""John"",
                            ""familyName"": ""Doe"",
                            ""metadata"": {
                                ""smsc.internalNumber"": ""456""
                            }
                        },
                        ""class"": {
                            ""classCode"": ""ABC123""
                        }
                    }
                ]
            }";

            // Act
            var result = await _jsonService.ExtractStudentInfoAsync(jsonData);

            // Assert
            Assert.Single(result);
            Assert.Equal("123", result[0].Identifier);
            Assert.Equal("John", result[0].GivenName);
            Assert.Equal("Doe", result[0].FamilyName);
            Assert.Equal("456", result[0].InternalNumber);
            Assert.Equal("ABC123", result[0].ClassCode);
        }
    }
}
