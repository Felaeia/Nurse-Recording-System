using Microsoft.AspNetCore.Http;
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
        private readonly Mock<IUserAuthenticationService> _mockAuthService;
        private readonly Mock<ISessionTokenService> _mockTokenService;
        private readonly AuthController _authController;

        public AuthControllerTest()
        {
            _mockAuthService = new Mock<IUserAuthenticationService>();
            _mockTokenService = new Mock<ISessionTokenService>();

            _authController = new AuthController(
                _mockAuthService.Object,
                _mockTokenService.Object
            )
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        // Successful login test with new token creation
        [Fact]
        public async Task LoginUser_ValidCredentials_NewToken_ReturnsOkResult()
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

            var expectedTokenResponse = new SessionTokenDTO
            {
                TokenId = 1,
                AuthId = expectedAuthResponse.AuthId,
                Token = new byte[] { 1, 2, 3, 4, 5 },
                ExpiresOn = DateTime.Now.AddHours(24)
            };

            _mockAuthService.Setup(service => service.AuthenticateAsync(It.IsAny<LoginRequestDTO>()))
                .ReturnsAsync(expectedAuthResponse);

            _mockTokenService.Setup(service => service.ValidateTokenAsync(expectedAuthResponse.AuthId))
                .ReturnsAsync(false); // No valid token, so create new

            _mockTokenService.Setup(service => service.CreateSessionAsync(expectedAuthResponse.AuthId))
                .ReturnsAsync(expectedTokenResponse);

            // --- Act ---
            var result = await _authController.LoginUser(loginRequest) as OkObjectResult;

            // --- Assert ---
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);

            dynamic data = result.Value;
            Assert.Equal(expectedAuthResponse, data.User);
            Assert.Equal(expectedTokenResponse, data.Token);
            Assert.Equal("Login Successful", data.Message);
        }

        // Successful login test with token refresh
        [Fact]
        public async Task LoginUser_ValidCredentials_RefreshToken_ReturnsOkResult()
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

            var expectedTokenResponse = new SessionTokenDTO
            {
                TokenId = 1,
                AuthId = expectedAuthResponse.AuthId,
                Token = new byte[] { 1, 2, 3, 4, 5 },
                ExpiresOn = DateTime.Now.AddHours(24)
            };

            _mockAuthService.Setup(service => service.AuthenticateAsync(It.IsAny<LoginRequestDTO>()))
                .ReturnsAsync(expectedAuthResponse);

            _mockTokenService.Setup(service => service.ValidateTokenAsync(expectedAuthResponse.AuthId))
                .ReturnsAsync(true); // Valid token exists, so refresh

            _mockTokenService.Setup(service => service.RefreshSessionTokenAsync(expectedAuthResponse.AuthId))
                .ReturnsAsync(expectedTokenResponse);

            // --- Act ---
            var result = await _authController.LoginUser(loginRequest) as OkObjectResult;

            // --- Assert ---
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);

            dynamic data = result.Value;
            Assert.Equal(expectedAuthResponse, data.User);
            Assert.Equal(expectedTokenResponse, data.Token);
            Assert.Equal("Login Successful", data.Message);
        }

        // Invalid credentials test
        [Fact]
        public async Task LoginUser_InvalidCredentials_ReturnsUnauthorized()
        {
            // --- Arrange ---
            var loginRequest = new LoginRequestDTO
            {
                Email = "wronguser@gmail.com",
                Password = "WrongPassword"
            };

            _mockAuthService.Setup(IUserAuthenticationService => IUserAuthenticationService.AuthenticateAsync(It.IsAny<LoginRequestDTO>()))
                .ReturnsAsync((LoginResponseDTO?)null);

            // --- Act ---
            var result = await _authController.LoginUser(loginRequest) as UnauthorizedObjectResult;

            // --- Assert ---
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
            Assert.Equal("Invalid credentials.", result.Value);
        }

        // Server error test
        [Fact]
        public async Task LoginUser_ExceptionThrown_ReturnsServerError()
        {
            // --- Arrange ---
            var loginRequest = new LoginRequestDTO
            {
                Email = "errormail@gmail.com",
                Password = "3123123"
            };

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
