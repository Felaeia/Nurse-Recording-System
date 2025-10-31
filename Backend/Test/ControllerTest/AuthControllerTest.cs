using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.Contracts.ServiceContracts.Auth;
using NurseRecordingSystem.Controllers.AuthenticationControllers;
using NurseRecordingSystem.DTO.AuthServiceDTOs;
using Xunit;

namespace NurseRecordingSystemTest.ControllerTest
{
    public class AuthControllerTest
    {
        // 1. Add mock for the new service
        private readonly Mock<IUserAuthenticationService> _mockAuthService;
        private readonly Mock<ISessionTokenService> _mockTokenService; // <-- ADDED
        private readonly AuthController _authController;

        public AuthControllerTest()
        {
            _mockAuthService = new Mock<IUserAuthenticationService>();
            _mockTokenService = new Mock<ISessionTokenService>(); // <-- ADDED

            // 2. Pass both mocks into the constructor
            _authController = new AuthController(
                _mockAuthService.Object,
                _mockTokenService.Object  // <-- ADDED
            );
        }

        //successful login test
        [Fact]
        public async Task LoginUser_ValidCredentials_ReturnsOkResult()
        {
            // --- Arrange ---
            var loginRequest = new LoginRequestDTO
            {
                Email = "testuser@gmail.com",
                Password = "Test@123"
            };

            var expectedAuthResponse = new LoginResponseDTO
            {
                AuthId = 1,
                Email = loginRequest.Email,
                UserName = "testuser",
                Role = "User",
                IsAuthenticated = true
            };

            // 3. Create a mock token response
            var expectedTokenResponse = new SessionTokenDTO
            {
                TokenId = 1,
                AuthId = expectedAuthResponse.AuthId,
                Token = new byte[] { 1, 2, 3, 4, 5 }, // Example token
                ExpiresOn = DateTime.Now.AddHours(24)
            };

            // Setup the mock for AuthenticateAsync
            _mockAuthService.Setup(service => service.AuthenticateAsync(It.IsAny<LoginRequestDTO>()))
                .ReturnsAsync(expectedAuthResponse);

            // 4. Setup the mock for CreateSessionAsync
            _mockTokenService.Setup(service => service.CreateSessionAsync(expectedAuthResponse.AuthId))
                .ReturnsAsync(expectedTokenResponse);

            // --- Act ---
            var result = await _authController.LoginUser(loginRequest) as OkObjectResult;

            // --- Assert ---
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);

            // 5. Check the anonymous object returned by the controller
            dynamic data = result.Value;
            Assert.Equal(expectedAuthResponse, data.User);
            Assert.Equal(expectedTokenResponse, data.Token);
            Assert.Equal("Login Successful", data.Message);
        }

        //invalid credentials test
        [Fact]
        public async Task LoginUser_InvalidCredentials_ReturnsUnauthorized()
        {
            // --- Arrange ---
            var loginRequest = new LoginRequestDTO
            {
                Email = "wronguser@gmail.com",
                Password = "WrongPassword"
            };

            // Setup auth service to return null (invalid credentials)
            _mockAuthService.Setup(s => s.AuthenticateAsync(It.IsAny<LoginRequestDTO>()))
                .ReturnsAsync((LoginResponseDTO?)null);

            // --- Act ---
            var result = await _authController.LoginUser(loginRequest) as UnauthorizedObjectResult;

            // --- Assert ---
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
            Assert.Equal("Invalid credentials.", result.Value);
        }

        //server error test
        [Fact]
        public async Task LoginUser_ExceptionThrown_ReturnsServerError()
        {
            // --- Arrange ---
            var loginRequest = new LoginRequestDTO
            {
                Email = "errormail@gmail.com",
                Password = "3123123"
            };

            // Setup auth service to throw an exception
            _mockAuthService.Setup(s => s.AuthenticateAsync(It.IsAny<LoginRequestDTO>()))
                .ThrowsAsync(new Exception("Database connection failed"));

            // --- Act ---
            var result = await _authController.LoginUser(loginRequest) as ObjectResult;

            // --- Assert ---
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Contains("Error in Login: Database connection failed", result.Value?.ToString());
        }
    }
}