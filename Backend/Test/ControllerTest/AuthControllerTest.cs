using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.Class.Services.Authentication;
using NurseRecordingSystem.Class.Services.HelperServices;
using NurseRecordingSystem.Contracts.ControllerContracts;
using NurseRecordingSystem.Contracts.RepositoryContracts.User;
using NurseRecordingSystem.Contracts.ServiceContracts.Auth;
using NurseRecordingSystem.Controllers;
using NurseRecordingSystem.Model.DTO.HelperDTOs;
using Xunit;

namespace NurseRecordingSystem.Test.ControllerTest
{
    public class AuthControllerTest
    {
        private readonly Mock<IUserAuthenticationService> _mockAuthenticationService;
        private readonly IAuthController _authController;

        public AuthControllerTest()
        {
            _mockAuthenticationService = new Mock<IUserAuthenticationService>();
            _authController = new AuthController(_mockAuthenticationService.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_ValidCredentials_ReturnsLoginResponseDTO()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPasswordHelper = new Mock<PasswordHelper>(); // You can mock the helper directly, but an interface is better.

            // Set up mock data
            var testUser = new User
            {
                // Populate with required properties
            };
            var loginRequest = new LoginRequestDTO
            {
                // Populate with test credentials
            };

            // Setup mock behavior
            mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync(It.IsAny<string>()))
                              .ReturnsAsync(testUser);
            mockPasswordHelper.Setup(helper => helper.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                              .Returns(true);

            var service = new UserAuthenticationService(mockConfiguration.Object, mockUserRepository.Object);

            // Act
            var result = await service.AuthenticateAsync(loginRequest);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsAuthenticated);
            Assert.Equal(testUser.Email, result.Email);
            // Add more asserts to check other properties
        }
    }
}
