using Microsoft.Extensions.Configuration;
using Moq;
using NurseRecordingSystem.Class.Services.Authentication;
using NurseRecordingSystem.Contracts.RepositoryContracts.User;
using NurseRecordingSystem.Contracts.ServiceContracts.Auth;
using NurseRecordingSystem.DTO.AuthServiceDTOs;

using Xunit;
using Microsoft.Data.SqlClient;
using System.Data;

namespace NurseRecordingSystem.Test.ServiceTests.AuthenticationServices
{
    public class UserAuthenticationServiceTest
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<IUserRepository> _mockUserRepo;
        private UserAuthenticationService _service;

        public UserAuthenticationServiceTest()
        {
            _mockConfig = new Mock<IConfiguration>();
            var mockConnectionStringsSection = new Mock<IConfigurationSection>();
            mockConnectionStringsSection.Setup(IConfigurationSection => IConfigurationSection["DefaultConnection"]).Returns("Server=test;Database=db;User Id=invalid;Password=invalid;Connection Timeout=1;");
            _mockConfig.Setup(IConfigurationSection => IConfigurationSection.GetSection("ConnectionStrings")).Returns(mockConnectionStringsSection.Object);
            _mockUserRepo = new Mock<IUserRepository>();
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenConnectionStringMissing()
        {
            // Arrange
            var badConfig = new Mock<IConfiguration>();
            var mockConnectionStringsSection = new Mock<IConfigurationSection>();
            mockConnectionStringsSection.Setup(x => x["DefaultConnection"]).Returns((string?)null);
            badConfig.Setup(x => x.GetSection("ConnectionStrings")).Returns(mockConnectionStringsSection.Object);
            var mockUserRepo = new Mock<IUserRepository>();

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() =>
                new UserAuthenticationService(badConfig.Object, mockUserRepo.Object)
            );

            Assert.Contains("Connection string 'DefaultConnection' not found", ex.Message);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenUserRepositoryNull()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            var mockConnectionStringsSection = new Mock<IConfigurationSection>();
            mockConnectionStringsSection.Setup(x => x["DefaultConnection"]).Returns("Server=test;Database=db;User Id=invalid;Password=invalid;Connection Timeout=1;");
            mockConfig.Setup(x => x.GetSection("ConnectionStrings")).Returns(mockConnectionStringsSection.Object);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new UserAuthenticationService(mockConfig.Object, null!)
            );

            Assert.Contains("UserAuth Service cannot be null", ex.Message);
        }

        [Fact]
        public void Constructor_ShouldSucceed_WhenDependenciesPresent()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            var mockConnectionStringsSection = new Mock<IConfigurationSection>();
            mockConnectionStringsSection.Setup(x => x["DefaultConnection"]).Returns("Server=test;Database=db;User Id=invalid;Password=invalid;Connection Timeout=1;");
            mockConfig.Setup(x => x.GetSection("ConnectionStrings")).Returns(mockConnectionStringsSection.Object);
            var mockUserRepo = new Mock<IUserRepository>();

            // Act
            var service = new UserAuthenticationService(mockConfig.Object, mockUserRepo.Object);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldThrow_WhenRequestNull()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            var mockConnectionStringsSection = new Mock<IConfigurationSection>();
            mockConnectionStringsSection.Setup(x => x["DefaultConnection"]).Returns("Server=test;Database=db;User Id=invalid;Password=invalid;Connection Timeout=1;");
            mockConfig.Setup(x => x.GetSection("ConnectionStrings")).Returns(mockConnectionStringsSection.Object);
            var mockUserRepo = new Mock<IUserRepository>();
            var service = new UserAuthenticationService(mockConfig.Object, mockUserRepo.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                service.AuthenticateAsync(null!)
            );

            Assert.Contains("LoginRequest cannot be Null", ex.Message);
        }

        [Fact]
        public async Task DetermineRoleAync_ShouldThrow_WhenResponseNull()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            var mockConnectionStringsSection = new Mock<IConfigurationSection>();
            mockConnectionStringsSection.Setup(x => x["DefaultConnection"]).Returns("Server=test;Database=db;User Id=invalid;Password=invalid;Connection Timeout=1;");
            mockConfig.Setup(x => x.GetSection("ConnectionStrings")).Returns(mockConnectionStringsSection.Object);
            var mockUserRepo = new Mock<IUserRepository>();
            var service = new UserAuthenticationService(mockConfig.Object, mockUserRepo.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                service.DetermineRoleAync(null!)
            );

            Assert.Contains("LoginResponse cannot be Null", ex.Message);
        }

        [Fact]
        public async Task DetermineRoleAync_ShouldReturnRole_WhenUserExists()
        {
            // Arrange
            var response = new LoginResponseDTO { UserName = "testuser" };
            var expectedRole = 1;

            _mockUserRepo.Setup(r => r.GetUserByUsernameAsync("testuser"))
                .ReturnsAsync(new UserAuthDTO { Role = expectedRole });

            _service = new UserAuthenticationService(_mockConfig.Object, _mockUserRepo.Object);

            // Act
            var result = await _service.DetermineRoleAync(response);

            // Assert
            Assert.Equal(expectedRole, result);
        }

        [Fact]
        public async Task DetermineRoleAync_ShouldThrow_WhenUserNotFound()
        {
            // Arrange
            var response = new LoginResponseDTO { UserName = "nonexistent" };

            _mockUserRepo.Setup(IUserRepository => IUserRepository.GetUserByUsernameAsync("nonexistent"))
                .ReturnsAsync((UserAuthDTO?)null);

            _service = new UserAuthenticationService(_mockConfig.Object, _mockUserRepo.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.DetermineRoleAync(response)
            );

            Assert.Contains("User not found", ex.Message);
        }
    }
}
