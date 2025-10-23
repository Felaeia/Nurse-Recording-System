using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.API.Controllers;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.NurseCreation;
using NurseRecordingSystem.Model.DTO.NurseServicesDTOs.NurseCreation;
using Xunit;

namespace NurseRecordingSystem.Test.ControllerTest
{
    public class AdminNurseControllerTest
    {
        private readonly Mock<ICreateNurse> _mockCreateNurseService;
        private readonly AdminNurseController _controller;

        public AdminNurseControllerTest()
        {
            _mockCreateNurseService = new Mock<ICreateNurse>();
            _controller = new AdminNurseController(_mockCreateNurseService.Object);
        }

        [Fact]
        public async Task RegisterNurse_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var request = new CreateNurseRequestDTO
            {
                UserName = "testnurse",
                Password = "TestPassword123",
                Email = "testnurse@example.com",
                FirstName = "John",
                MiddleName = "Doe",
                LastName = "Smith",
                ContactNumber = "1234567890"
            };
            var newAuthId = 123;

            _mockCreateNurseService.Setup(s => s.CreateNurseAsync(request)).ReturnsAsync(newAuthId);

            // Act
            var result = await _controller.RegisterNurse(request) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(nameof(_controller.RegisterNurse), result.ActionName);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task RegisterNurse_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateNurseRequestDTO(); // Invalid, missing required fields
            _controller.ModelState.AddModelError("UserName", "UserName is required");

            // Act
            var result = await _controller.RegisterNurse(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task RegisterNurse_ValidRequestWithInvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateNurseRequestDTO
            {
                UserName = "testnurse",
                Password = "TestPassword123",
                Email = "testnurse@example.com",
                FirstName = "John",
                LastName = "Smith"
            };
            _controller.ModelState.AddModelError("UserName", "UserName is invalid");

            // Act
            var result = await _controller.RegisterNurse(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task RegisterNurse_ConflictException_ReturnsConflict()
        {
            // Arrange
            var request = new CreateNurseRequestDTO
            {
                UserName = "existingnurse",
                Password = "TestPassword123",
                Email = "existing@example.com",
                FirstName = "Jane",
                LastName = "Doe"
            };

            _mockCreateNurseService.Setup(ICreateNurse => ICreateNurse.CreateNurseAsync(request)).ThrowsAsync(new InvalidOperationException("Email already exists"));

            // Act
            var result = await _controller.RegisterNurse(request) as ConflictObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(409, result.StatusCode);
            Assert.Contains("Email already exists", result.Value?.ToString());
        }

        [Fact]
        public async Task RegisterNurse_GeneralException_ReturnsInternalServerError()
        {
            // Arrange
            var request = new CreateNurseRequestDTO
            {
                UserName = "testnurse",
                Password = "TestPassword123",
                Email = "testnurse@example.com",
                FirstName = "John",
                LastName = "Smith"
            };

            _mockCreateNurseService.Setup(ICreateNurse => ICreateNurse.CreateNurseAsync(request)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.RegisterNurse(request) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Account creation failed due to internal error", result.Value?.ToString());
        }
    }
}
