
using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.Contracts.ServiceContracts.Auth;
using NurseRecordingSystem.Controllers;
using NurseRecordingSystem.Model.DTO.HelperDTOs;
using Xunit;

namespace NurseRecordingSystemTest.ControllerTest
{
    public class AuthControllerTest
    {
        private readonly Mock<IUserAuthenticationService> _mockAuthService;
        private readonly AuthController _authController;

        public AuthControllerTest()
        {
            _mockAuthService = new Mock<IUserAuthenticationService>();
            _authController = new AuthController(_mockAuthService.Object);
        }
        //successful login test
        [Fact]
        public async Task LoginUser_ValidCredentials_ReturnsOkResult()
        {

            var loginRequest = new LoginRequestDTO
            {
                Email = "testuser@gmail.com",
                Password = "Test@123"
            };
            var expectedResponse = new LoginResponseDTO 
            {
                AuthId = 1,
                Email = loginRequest.Email,
                UserName = "testuser",
                Role = 1,
                IsAuthenticated = true
            };

            _mockAuthService.Setup(s => s.AuthenticateAsync(It.IsAny<LoginRequestDTO>())).ReturnsAsync(expectedResponse);


            var result = await _authController.LoginUser(loginRequest) as OkObjectResult;


            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Contains("Login Succesful", result.Value?.ToString());
        }
        //invalid credentials test
        [Fact]
        public async Task LoginUser_InvalidCredentials_ReturnsUnauthorized()
        {

            var loginRequest = new LoginRequestDTO
            {
                Email = "wronguser@gmail.com",
                Password = "WrongPassword"
            };
            _mockAuthService.Setup(iuserauthservice => iuserauthservice.AuthenticateAsync(It.IsAny<LoginRequestDTO>())).ReturnsAsync((LoginResponseDTO?)null);



            var result = await _authController.LoginUser(loginRequest) as UnauthorizedObjectResult;


            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
            Assert.Equal("Invalid credentials.", result.Value);
        }

        //server error test
        [Fact]
        public async Task LoginUser_ExceptionThrown_ReturnsServerError()
        {

            var loginRequest = new LoginRequestDTO
            {
                Email = "errormail@gmail.com",
                Password = "3123123"
            };
            _mockAuthService.Setup(s => s.AuthenticateAsync(It.IsAny<LoginRequestDTO>())).ThrowsAsync(new Exception("Testing exception"));


            var result = await _authController.LoginUser(loginRequest) as ObjectResult;


            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Contains("Error in Login", result.Value?.ToString());
            //sdsdfsfds
        }
    }
}