using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.Contracts.ServiceContracts.IHelperServices.IHelperClinicStatus;
using NurseRecordingSystem.DTO.HelperServiceDTOs.HelperClinicStatusDTOs;
using Xunit;

namespace NurseRecordingSystem.Test.ControllerTest.HelperControllerTest
{
    public class HelperClinicStatusControllerTest
    {
        private readonly Mock<IViewClinicStatus> _mockViewService;
        private readonly HelperClinicStatusController _controller;

        public HelperClinicStatusControllerTest()
        {
            _mockViewService = new Mock<IViewClinicStatus>();
            _controller = new HelperClinicStatusController(_mockViewService.Object);
        }

        [Fact]
        public async Task ViewAllStatus_SuccessfulView_ReturnsOk()
        {
            // Arrange
            var expectedList = new List<ViewClinicStatusResponseDTO>
            {
                new ViewClinicStatusResponseDTO { LogId = 1, Status = true },
                new ViewClinicStatusResponseDTO { LogId = 2, Status = false }
            };
            _mockViewService.Setup(s => s.ViewAllAsync()).ReturnsAsync(expectedList);

            // Act
            var result = await _controller.ViewAllStatus() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedList, result.Value);
        }

        [Fact]
        public async Task ViewAllStatus_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            _mockViewService.Setup(s => s.ViewAllAsync()).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.ViewAllStatus() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Database error", result.Value?.ToString());
        }
    }
}
