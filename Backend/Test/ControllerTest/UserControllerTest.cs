using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.Contracts.ControllerContracts;
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
        private readonly IUserController _userController;

        public UserControllerTest()
        {
            _mockCreateUserService = new Mock<ICreateUsersService>();
            _userController = new UserController(_mockCreateUserService.Object);
        }
        [Fact]
        public async Task CreateUser_ValidRequest_ReturnsOkResult()
        {
            // Mock Data 
            var userAuth = new CreateUserWithAuthenticationDTO
            {
                UserName = "Mags",
                Password = "123123",
                Email = "testuser@gmail.com",
                FirstName = "Test",
                MiddleName = "T",
                LastName = "User",
                ContactNumber = "1234567890",
                Address = "LLC"
            };
            var request = new CreateAuthenticationRequestDTO
            {
                UserName = userAuth.Email,
                Password = userAuth.Password,
                Email = userAuth.Email
            };
            var user = new CreateUserRequestDTO
            {
                FirstName = userAuth.FirstName,
                MiddleName = userAuth.MiddleName,
                LastName = userAuth.LastName,
                ContactNumber = userAuth.ContactNumber,
                Address = userAuth.Address
            };

            var random = new Random();
            var expectedAuthId = random.Next(100);
            _mockCreateUserService.Setup(s => s.CreateUserAuthenticateAsync(request, user)).ReturnsAsync(expectedAuthId);
            _mockCreateUserService.Setup(s => s.CreateUser(user)).Returns(Task.CompletedTask);

            var result = await _userController.CreateAuthentication(userAuth) as OkObjectResult;


            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }
    }
}
