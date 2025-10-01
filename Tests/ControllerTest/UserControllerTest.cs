using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.Contracts.ServiceContracts.User;
using NurseRecordingSystem.Model.DTO;
using PresentationProject.Controllers;

namespace NurseRecordingSystemTest.ControllerTest
{
    public class UserControllerTest
    {
        private readonly Mock<ICreateUsersService> _mockCreateUserService;
        private readonly UserController _userController;

        public UserControllerTest()
        {
            _mockCreateUserService = new Mock<ICreateUsersService>();
            _userController = new UserController(_mockCreateUserService.Object);
        }
        [Fact]
        public async Task CreateAuthentication_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new CreateAuthenticationRequest
            {
                UserName = "testuser",
                Password = "Test@123",
                Email = "testuser@gmail.com"
            };
            var random = new Random();
            var expectedAuthId = random.Next(100);
            _mockCreateUserService.Setup(s => s.CreateAuthenticationAsync(request)).ReturnsAsync(expectedAuthId);

            // Act

            var result = await _userController.CreateAuthentication(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }
    }
}
