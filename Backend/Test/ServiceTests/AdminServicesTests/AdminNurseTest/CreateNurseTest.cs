using NurseRecordingSystem.Class.Services.NurseServices.NurseCreation;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Xunit;
using Moq;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using NurseRecordingSystem.Model.DTO.NurseServicesDTOs.NurseCreation;

namespace NurseRecordingSystem.Test.ServiceTests.AdminServicesTests.AdminNurseTest
{
    public class CreateNurseTest
    {
        [Fact]
        public void Constructor_ConfigurationNull_ThrowsInvalidOperationException()
        {
            // Arrange
            IConfiguration config = null;

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => new CreateNurse(config));
            Assert.Contains("Connection string 'DefaultConnection' not found.", exception.Message);
        }

        [Fact]
        public void Constructor_ConnectionStringNull_ThrowsInvalidOperationException()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = null })
                .Build();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => new CreateNurse(config));
            Assert.Contains("Connection string 'DefaultConnection' not found.", exception.Message);
        }

        [Fact]
        public async Task CreateNurseAsync_RequestNull_ThrowsArgumentNullException()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = "fakeConnection" })
                .Build();
            var service = new CreateNurse(config);
            CreateNurseRequestDTO request = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateNurseAsync(request));
        }

        [Fact]
        public async Task CreateNurseAsync_InvalidConnection_ThrowsException()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> {
                {"ConnectionStrings:DefaultConnection", "Server=(localdb)//MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1;"}
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var service = new CreateNurse(config);
            var request = new CreateNurseRequestDTO
            {
                UserName = "testuser",
                Password = "password",
                Email = "test@example.com",
                FirstName = "Test",
                MiddleName = "TEst",
                LastName = "Test",
                ContactNumber = "1234567890"
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.CreateNurseAsync(request));
        }

    }
}
