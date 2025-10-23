using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.Controllers.AdminControllers;
using NurseRecordingSystem.Contracts.ServiceContracts.IAdminServices.IAdminUser;
using Xunit;

namespace NurseRecordingSystem.Test.ControllerTest.AdminControllerTest
{
    public class AdminUsersControllerTest
    {
        private readonly Mock<IDeleteUser> _mockDeleteService;
        private readonly AdminUsersController _controller;

        public AdminUsersControllerTest()
        {
            _mockDeleteService = new Mock<IDeleteUser>();
            _controller = new AdminUsersController(_mockDeleteService.Object);
        }

        [Fact]
        public async Task SoftDeleteUser_SuccessfulDeletion_ReturnsNoContent()
        {
            // Arrange
            var userId = 123;
            var deletedBy = "AdminSystem";
            _mockDeleteService.Setup(IDeleteUser => IDeleteUser.SoftDeleteUserAsync(userId, deletedBy)).ReturnsAsync(true);

            // Act
            var result = await _controller.SoftDeleteUser(userId, deletedBy) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task SoftDeleteUser_InvalidUserId_ReturnsBadRequest()
        {
            // Arrange
            var userId = 0;
            var deletedBy = "AdminSystem";

            // Act
            var result = await _controller.SoftDeleteUser(userId, deletedBy) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("Invalid user ID", result.Value?.ToString());
        }

        [Fact]
        public async Task SoftDeleteUser_MissingDeletedBy_ReturnsBadRequest()
        {
            // Arrange
            var userId = 123;
            string deletedBy = null;

            // Act
            var result = await _controller.SoftDeleteUser(userId, deletedBy) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("X-Deleted-By", result.Value?.ToString());
        }

        [Fact]
        public async Task SoftDeleteUser_NotFound_ReturnsNotFound()
        {
            // Arrange
            var userId = 999;
            var deletedBy = "AdminSystem";
            _mockDeleteService.Setup(IDeleteUser => IDeleteUser.SoftDeleteUserAsync(userId, deletedBy)).ThrowsAsync(new Exception("User not found"));

            // Act
            var result = await _controller.SoftDeleteUser(userId, deletedBy) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Contains("not found", result.Value?.ToString());
        }

        [Fact]
        public async Task SoftDeleteUser_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var userId = 123;
            var deletedBy = "AdminSystem";
            _mockDeleteService.Setup(IDeleteUser => IDeleteUser.SoftDeleteUserAsync(userId, deletedBy)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.SoftDeleteUser(userId, deletedBy) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("error occurred", result.Value?.ToString());
        }
    }
}
