using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.Controllers.AdminControllers;
using NurseRecordingSystem.Contracts.ServiceContracts.IAdminServices.IAdminClinicStatus;
using NurseRecordingSystem.DTO.AdminServiceDTOs.AdminClinicStatusDTOs;
using Xunit;

namespace NurseRecordingSystem.Test.ControllerTest
{
    public class AdminClinicStatusControllerTest
    {
        private readonly Mock<ICreateClinicStatus> _mockCreateService;
        private readonly Mock<IDeleteClinicStatus> _mockDeleteService;
        private readonly AdminClinicStatusController _controller;

        public AdminClinicStatusControllerTest()
        {
            _mockCreateService = new Mock<ICreateClinicStatus>();
            _mockDeleteService = new Mock<IDeleteClinicStatus>();
            _controller = new AdminClinicStatusController(_mockCreateService.Object, _mockDeleteService.Object);
        }

        [Fact]
        public async Task CreateStatus_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var request = new CreateClinicStatusRequestDTO
            {
                Status = true,
                NurseId = 1
            };
            var createdBy = "AdminSystem";
            var newId = 123;

            _mockCreateService.Setup(IDeleteClinicStatus => IDeleteClinicStatus.CreateAsync(request, createdBy)).ReturnsAsync(newId);

            // Act
            var result = await _controller.CreateStatus(request) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(nameof(_controller.CreateStatus), result.ActionName);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task CreateStatus_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateClinicStatusRequestDTO(); // Invalid, missing required fields
            _controller.ModelState.AddModelError("ClinicId", "ClinicId is required");

            // Act
            var result = await _controller.CreateStatus(request) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task CreateStatus_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var request = new CreateClinicStatusRequestDTO
            {
                Status = true,
                NurseId = 1
            };
            var createdBy = "AdminSystem";

            _mockCreateService.Setup(IDeleteClinicStatus => IDeleteClinicStatus.CreateAsync(request, createdBy)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.CreateStatus(request) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Internal server error", result.Value?.ToString());
        }

        [Fact]
        public async Task DeleteStatus_SuccessfulDeletion_ReturnsNoContent()
        {
            // Arrange
            var id = 123;
            var deletedBy = "AdminSystem";

            _mockDeleteService.Setup(IDeleteClinicStatus => IDeleteClinicStatus.DeleteAsync(id, deletedBy)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteStatus(id) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task DeleteStatus_NotFound_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var deletedBy = "AdminSystem";

            _mockDeleteService.Setup(IDeleteClinicStatus => IDeleteClinicStatus.DeleteAsync(id, deletedBy)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteStatus(id) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Contains("not found", result.Value?.ToString());
        }

        [Fact]
        public async Task DeleteStatus_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var id = 123;
            var deletedBy = "AdminSystem";

            _mockDeleteService.Setup(IDeleteClinicStatus => IDeleteClinicStatus.DeleteAsync(id, deletedBy)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.DeleteStatus(id) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Internal server error", result.Value?.ToString());
        }
    }
}
