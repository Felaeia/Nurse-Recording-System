using Microsoft.Extensions.Configuration;
using Moq;
using NurseRecordingSystem.Class.Services.UserServices.Users;
using NurseRecordingSystem.DTO.UserServiceDTOs.UsersDTOs;
using Xunit;

namespace NurseRecordingSystem.Test.ServiceTests.UserServicesTests.UserServicesTests
{
    public class UpdateUserTest
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly UpdateUser _service;

        public UpdateUserTest()
        {
            _mockConfig = new Mock<IConfiguration>();
            var mockConnectionStringsSection = new Mock<IConfigurationSection>();
            mockConnectionStringsSection.Setup(x => x["DefaultConnection"]).Returns("Server=test;Database=db;User Id=invalid;Password=invalid;Connection Timeout=1;");
            _mockConfig.Setup(x => x.GetSection("ConnectionStrings")).Returns(mockConnectionStringsSection.Object);
            _service = new UpdateUser(_mockConfig.Object);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenConnectionStringMissing()
        {
            // Arrange
            var badConfig = new Mock<IConfiguration>();
            var mockConnectionStringsSection = new Mock<IConfigurationSection>();
            mockConnectionStringsSection.Setup(x => x["DefaultConnection"]).Returns((string?)null);
            badConfig.Setup(x => x.GetSection("ConnectionStrings")).Returns(mockConnectionStringsSection.Object);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() =>
                new UpdateUser(badConfig.Object)
            );

            Assert.Contains("Connection string 'DefaultConnection' not found", ex.Message);
        }

        [Fact]
        public void Constructor_ShouldSucceed_WhenDependenciesPresent()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            var mockConnectionStringsSection = new Mock<IConfigurationSection>();
            mockConnectionStringsSection.Setup(x => x["DefaultConnection"]).Returns("Server=test;Database=db;User Id=invalid;Password=invalid;Connection Timeout=1;");
            mockConfig.Setup(x => x.GetSection("ConnectionStrings")).Returns(mockConnectionStringsSection.Object);

            // Act
            var service = new UpdateUser(mockConfig.Object);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public async Task UpdateUserProfileAsync_ShouldThrow_WhenRequestNull()
        {
            // Arrange
            var userId = 1;
            var updatedBy = "testuser";

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _service.UpdateUserProfileAsync(userId, null!, updatedBy)
            );

            Assert.Contains("User request cannot be null", ex.Message);
        }

        [Fact]
        public async Task UpdateUserProfileAsync_ShouldThrow_WhenUpdatedByNullOrEmpty()
        {
            // Arrange
            var userId = 1;
            var userRequest = new UpdateUserRequestDTO { Email = "test@example.com", FirstName = "Test", LastName = "User" };

            // Act & Assert for null
            var ex1 = await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.UpdateUserProfileAsync(userId, userRequest, null!)
            );

            Assert.Contains("Updated by cannot be null or empty", ex1.Message);

            // Act & Assert for empty
            var ex2 = await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.UpdateUserProfileAsync(userId, userRequest, "")
            );

            Assert.Contains("Updated by cannot be null or empty", ex2.Message);
        }

        [Fact]
        public async Task UpdateUserProfileAsync_ShouldThrow_WhenUserIdInvalid()
        {
            // Arrange
            var userId = 0;
            var userRequest = new UpdateUserRequestDTO { Email = "test@example.com", FirstName = "Test", LastName = "User" };
            var updatedBy = "testuser";

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.UpdateUserProfileAsync(userId, userRequest, updatedBy)
            );

            Assert.Contains("User ID must be greater than zero", ex.Message);
        }


    }
}
