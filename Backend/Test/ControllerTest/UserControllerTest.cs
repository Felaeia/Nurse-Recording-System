using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.Contracts.ServiceContracts.User;
using NurseRecordingSystem.Model.DTO.AuthDTOs;
using NurseRecordingSystem.Model.DTO.UserDTOs;
using PresentationProject.Controllers;
using Xunit;

namespace NurseRecordingSystemTest.ControllerTest
{
    public class UserControllerTest
    {
        private readonly Mock<ICreateUsersService> _mockCreateUserService;
        private readonly CreateUserController _userController;

        public UserControllerTest()
        {
            _mockCreateUserService = new Mock<ICreateUsersService>();
            _userController = new CreateUserController(_mockCreateUserService.Object);
        }
        [Fact]
        public async Task CreateAuthentication_ValidRequest_ReturnsOkResult()
        {
            // ARRANGE

            // 1. Define the inputs for the test
            var combinedRequest = new CreateUserWithAuthenticationDTO // Use the DTO accepted by the Controller's public method
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

            // 2. Define the expected return value from the service
            const int expectedAuthId = 42;

            // 3. Setup the mock service to return the expected value
            // NOTE: The controller logic breaks down the combined DTO into two separate DTOs 
            // before calling the service, so we use It.IsAny to match the call.
            _mockCreateUserService
                .Setup(s => s.CreateUserAuthenticateAsync(
                    It.IsAny<CreateAuthenticationRequestDTO>(),
                    It.IsAny<CreateUserRequestDTO>()
                ))
                // Set the return value here!
                .ReturnsAsync(expectedAuthId);

            // ACT
            // Call the public controller method with the ONE COMBINED DTO.
            var result = await _userController.CreateAuthentication(combinedRequest) as OkObjectResult;

            // ASSERT
            // 1. Check if the result is not null and is an OkObjectResult
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            // 2. Check the content of the OkResult
            // Since your controller returns: new { AuthId = authId, Message = "..." }
            // we need to cast the result.Value to a dynamic type to access properties.
            dynamic resultValue = result.Value;
            Assert.NotNull(resultValue);
            Assert.Equal(expectedAuthId, resultValue.AuthId); // Verify the ID was returned
            Assert.Equal("Authentication created successfully.", resultValue.Message);
        }
    }
}
