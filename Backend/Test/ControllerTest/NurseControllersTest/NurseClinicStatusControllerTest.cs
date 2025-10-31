using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseClinicStatus;
using NurseRecordingSystem.Controllers.NurseControllers;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseClinicStatusDTOs;
using System.Security.Claims;
using Xunit;

namespace NurseRecordingSystem.Test.ControllerTest.NurseControllersTest
{
    public class NurseClinicStatusControllerTest
    {
        private readonly Mock<IUpdateClinicStatus> _mockUpdateService;
        private readonly NurseClinicStatusController _controller;

        public NurseClinicStatusControllerTest()
        {
            _mockUpdateService = new Mock<IUpdateClinicStatus>();
            _controller = new NurseClinicStatusController(_mockUpdateService.Object);
        }

        [Fact]
        public async Task UpdateStatus_ValidRequest_ReturnsNoContent()
        {
            // Arrange
            var id = 1;
            var request = new UpdateClinicStatusRequestDTO { Status = true };
            var updatedBy = "Nurse1";

            // Mock the user identity
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, updatedBy) };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };

            _mockUpdateService.Setup(service => service.UpdateAsync(id, request, updatedBy)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateStatus(id, request) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task UpdateStatus_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var id = 1;
            var request = new UpdateClinicStatusRequestDTO();
            _controller.ModelState.AddModelError("Status", "Required");

            // Act
            var result = await _controller.UpdateStatus(id, request) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task UpdateStatus_NotFound_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var request = new UpdateClinicStatusRequestDTO { Status = false };
            var updatedBy = "NurseSystem";

            _mockUpdateService.Setup(service => service.UpdateAsync(id, request, updatedBy)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateStatus(id, request) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Contains($"Clinic Status Log with ID {id} not found or is inactive.", result.Value?.ToString());
        }

        [Fact]
        public async Task UpdateStatus_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var id = 1;
            var request = new UpdateClinicStatusRequestDTO { Status = true };
            var updatedBy = "NurseSystem";

            _mockUpdateService.Setup(service => service.UpdateAsync(id, request, updatedBy)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.UpdateStatus(id, request) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Internal server error: Database error", result.Value?.ToString());
        }

        [Fact]
        public async Task UpdateStatus_UserIdentityNull_UsesDefaultUpdatedBy()
        {
            // Arrange
            var id = 1;
            var request = new UpdateClinicStatusRequestDTO { Status = true };
            var updatedBy = "NurseSystem"; // Default value

            // No user set, so User?.Identity?.Name is null
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = null }
            };

            _mockUpdateService.Setup(service => service.UpdateAsync(id, request, updatedBy)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateStatus(id, request) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }
    }
}
