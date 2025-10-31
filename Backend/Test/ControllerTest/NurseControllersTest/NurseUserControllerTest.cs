using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.Controllers.NurseControllers;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseUsers;
using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UsersDTOs;
using System.Collections.Generic;
using Xunit;

namespace NurseRecordingSystem.Test.ControllerTest.NurseControllersTest
{
    public class NurseUserControllerTest
    {
        private readonly Mock<IViewAllUsers> _mockViewAllUsersService;
        private readonly NurseUserController _controller;

        public NurseUserControllerTest()
        {
            _mockViewAllUsersService = new Mock<IViewAllUsers>();
            _controller = new NurseUserController(_mockViewAllUsersService.Object);
        }

        [Fact]
        public async Task GetAllUsers_ValidRequestWithoutIsActive_ReturnsOk()
        {
            // Arrange
            var users = new List<ViewUserDTO> { new ViewUserDTO { /* populate */ } };
            _mockViewAllUsersService.Setup(service => service.GetAllUsersAsync(null)).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers(null) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(users, result.Value);
        }

        [Fact]
        public async Task GetAllUsers_ValidRequestWithIsActiveTrue_ReturnsOk()
        {
            // Arrange
            var users = new List<ViewUserDTO> { new ViewUserDTO { /* populate */ } };
            _mockViewAllUsersService.Setup(service => service.GetAllUsersAsync(true)).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers(true) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(users, result.Value);
        }

        [Fact]
        public async Task GetAllUsers_ValidRequestWithIsActiveFalse_ReturnsOk()
        {
            // Arrange
            var users = new List<ViewUserDTO> { new ViewUserDTO { /* populate */ } };
            _mockViewAllUsersService.Setup(service => service.GetAllUsersAsync(false)).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers(false) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(users, result.Value);
        }

        [Fact]
        public async Task GetAllUsers_EmptyList_ReturnsOkWithEmptyList()
        {
            // Arrange
            var users = new List<ViewUserDTO>();
            _mockViewAllUsersService.Setup(service => service.GetAllUsersAsync(null)).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers(null) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(users, result.Value);
        }

        [Fact]
        public async Task GetAllUsers_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            _mockViewAllUsersService.Setup(service => service.GetAllUsersAsync(null)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetAllUsers(null) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("An error occurred while retrieving the list of users.", result.Value?.ToString());
        }
    }
}
