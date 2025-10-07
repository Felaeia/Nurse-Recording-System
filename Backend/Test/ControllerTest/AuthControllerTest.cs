
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
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using NurseRecordingSystem.Controllers;
//using NurseRecordingSystem.Model.DTO.HelperDTOs;
//using NurseRecordingSystem.Contracts.ServiceContracts.Auth;
//using Xunit;

//namespace NurseRecordingSystem.Test.ControllerTest
//{
//    public class AuthControllerTest
//    {
//        // Mock the dependency (the service)
//        private readonly Mock<IUserAuthenticationService> _mockAuthenticationService;

//        // The controller instance being tested
//        private readonly AuthController _authController;

//        public AuthControllerTest()
//        {
//            _mockAuthenticationService = new Mock<IUserAuthenticationService>();
//            // AuthController must be instantiated as a concrete class for testing
//            _authController = new AuthController(_mockAuthenticationService.Object);
//        }

//        // ------------------------------------------------------------------
//        // TEST CASE 1: Successful Login
//        // ------------------------------------------------------------------

//        [Fact]
//        public async Task LoginUser_ValidCredentials_ReturnsOkResultAndLoginResponse()
//        {
//            // ARRANGE
//            var loginRequest = new LoginRequestDTO
//            {
//                Email = "test@user.com",
//                Password = "Test@123"
//            };

//            var expectedResponse = new LoginResponseDTO
//            {
//                AuthId = 1,
//                Email = loginRequest.Email,
//                UserName = "testuser",
//                Role = 1,
//                IsAuthenticated = true
//            };

//            // Setup the service mock: When AuthenticateAsync is called, return the expected success response
//            _mockAuthenticationService
//                .Setup(s => s.AuthenticateAsync(
//                    It.Is<LoginRequestDTO>(r => r.Email == loginRequest.Email && r.Password == loginRequest.Password)
//                ))
//                .ReturnsAsync(expectedResponse);

//            // ACT
//            // Call the controller method
//            var result = await _authController.LoginUser(loginRequest);

//            // ASSERT
//            // 1. Check the result type and status code
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.Equal(200, okResult.StatusCode);

//            // 2. Check the content structure
//            // Your controller returns: new { Response = response, Message = "Login Succesful" }
//            dynamic resultValue = okResult.Value!;

//            // 3. Verify the message and the inner response object
//            Assert.Equal("Login Succesful", resultValue.Message);

//            // 4. Verify the core data returned matches the mock
//            var actualResponse = Assert.IsType<LoginResponseDTO>(resultValue.Response);
//            Assert.Equal(expectedResponse.Email, actualResponse.Email);
//            Assert.True(actualResponse.IsAuthenticated);

//            // 5. Verify the service was called once
//            _mockAuthenticationService.Verify(s => s.AuthenticateAsync(It.IsAny<LoginRequestDTO>()), Times.Once);
//        }

//        // ------------------------------------------------------------------
//        // TEST CASE 2: Invalid Credentials (Unauthorized)
//        // ------------------------------------------------------------------

//        [Fact]
//        public async Task LoginUser_InvalidCredentials_ReturnsUnauthorized()
//        {
//            // ARRANGE
//            var loginRequest = new LoginRequestDTO
//            {
//                Email = "wrong@user.com",
//                Password = "WrongPassword"
//            };

//            // Setup the service mock to throw the same exception the service would throw 
//            // on failure (based on your service logic, it throws UnauthorizedAccessException 
//            // or returns null if the query failed).
//            _mockAuthenticationService
//                .Setup(s => s.AuthenticateAsync(It.IsAny<LoginRequestDTO>()))
//                // We'll mock it to throw an UnauthorizedAccessException, which your controller 
//                // implicitly handles via the catch block (though it's better to return null 
//                // or a simple failure response from the service).
//                .ThrowsAsync(new UnauthorizedAccessException("Invalid Email or Password."));

//            // ACT
//            var result = await _authController.LoginUser(loginRequest);

//            // ASSERT
//            // 1. Check the result type and status code (Your controller returns a 500 on exceptions)
//            var statusCodeResult = Assert.IsType<ObjectResult>(result);
//            Assert.Equal(500, statusCodeResult.StatusCode); // Based on your controller's catch block

//            // 2. Verify the service was called once
//            _mockAuthenticationService.Verify(s => s.AuthenticateAsync(It.IsAny<LoginRequestDTO>()), Times.Once);
//        }
//    }
//}