using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.Users;
using NurseRecordingSystem.Controllers.UserControllers;
using NurseRecordingSystem.DTO.UserServiceDTOs.UsersDTOs;
using Xunit;

namespace NurseRecordingSystemTest.ControllerTest
{
    public class UserControllerTest
    {
        private readonly Mock<ICreateUsers> _mockCreateUserService;
        private readonly CreateUserController _userController;

        public UserControllerTest()
        {
            _mockCreateUserService = new Mock<ICreateUsers>();
            _userController = new CreateUserController(_mockCreateUserService.Object);
        }
        [Fact]
        public async Task CreateAuthentication_ValidRequest_ReturnsOkResult()
        {
            // ARRANGE

            // 1. Define the inputs for the test
            var combinedRequest = new CreateAuthenticationRequestDTO // Use the DTO accepted by the Controller's public method
            {
                UserName = "testuser",
                Password = "Test@123",
                Email = "testuser@gmail.com",
                FirstName = "Test",
                MiddleName = "T",
                LastName = "User",
                ContactNumber = "1234567890",
                Address = "123 Test St"
            };

            const int expectedAuthId = 42;

            _mockCreateUserService
                .Setup(s => s.CreateUserAuthenticateAsync(
                    It.IsAny<CreateAuthenticationRequestDTO>(),
                    It.IsAny<CreateUserRequestDTO>()
                ))

                .ReturnsAsync(expectedAuthId);


            var result = await _userController.CreateAuthentication(combinedRequest) as OkObjectResult;


            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);


            dynamic resultValue = result.Value;
            Assert.NotNull(resultValue);
            Assert.Equal(expectedAuthId, resultValue.AuthId); 
            Assert.Equal("Authentication created successfully.", resultValue.Message);
        }
    }
}
