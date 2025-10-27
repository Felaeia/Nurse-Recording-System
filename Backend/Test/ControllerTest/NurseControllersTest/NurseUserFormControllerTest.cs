using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.API.Controllers;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseUserForms;
using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UserFormsDTOs;
using System.Collections.Generic;
using Xunit;

namespace NurseRecordingSystem.Test.ControllerTest.NurseControllersTest
{
    public class NurseUserFormControllerTest
    {
        private readonly Mock<IViewUserFormList> _mockViewUserFormListService;
        private readonly NurseUserFormController _controller;

        public NurseUserFormControllerTest()
        {
            _mockViewUserFormListService = new Mock<IViewUserFormList>();
            _controller = new NurseUserFormController(_mockViewUserFormListService.Object);
        }

        [Fact]
        public async Task GetUserFormList_ValidRequestWithoutFilters_ReturnsOk()
        {
            // Arrange
            var forms = new List<UserFormListItemDTO> { new UserFormListItemDTO { /* populate */ } };
            _mockViewUserFormListService.Setup(service => service.GetUserFormListAsync(null, null)).ReturnsAsync(forms);

            // Act
            var result = await _controller.GetUserFormList(null, null) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(forms, result.Value);
        }

        [Fact]
        public async Task GetUserFormList_ValidRequestWithUserId_ReturnsOk()
        {
            // Arrange
            var userId = 101;
            var forms = new List<UserFormListItemDTO> { new UserFormListItemDTO { /* populate */ } };
            _mockViewUserFormListService.Setup(service => service.GetUserFormListAsync(userId, null)).ReturnsAsync(forms);

            // Act
            var result = await _controller.GetUserFormList(userId, null) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(forms, result.Value);
        }

        [Fact]
        public async Task GetUserFormList_ValidRequestWithStatus_ReturnsOk()
        {
            // Arrange
            var status = "Pending";
            var forms = new List<UserFormListItemDTO> { new UserFormListItemDTO { /* populate */ } };
            _mockViewUserFormListService.Setup(service => service.GetUserFormListAsync(null, status)).ReturnsAsync(forms);

            // Act
            var result = await _controller.GetUserFormList(null, status) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(forms, result.Value);
        }

        [Fact]
        public async Task GetUserFormList_ValidRequestWithBothFilters_ReturnsOk()
        {
            // Arrange
            var userId = 101;
            var status = "Pending";
            var forms = new List<UserFormListItemDTO> { new UserFormListItemDTO { /* populate */ } };
            _mockViewUserFormListService.Setup(service => service.GetUserFormListAsync(userId, status)).ReturnsAsync(forms);

            // Act
            var result = await _controller.GetUserFormList(userId, status) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(forms, result.Value);
        }

        [Fact]
        public async Task GetUserFormList_EmptyList_ReturnsOkWithEmptyList()
        {
            // Arrange
            var forms = new List<UserFormListItemDTO>();
            _mockViewUserFormListService.Setup(service => service.GetUserFormListAsync(null, null)).ReturnsAsync(forms);

            // Act
            var result = await _controller.GetUserFormList(null, null) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(forms, result.Value);
        }

        [Fact]
        public async Task GetUserFormList_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            _mockViewUserFormListService.Setup(service => service.GetUserFormListAsync(null, null)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetUserFormList(null, null) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("An error occurred while retrieving the list of patient forms.", result.Value?.ToString());
        }
    }
}
