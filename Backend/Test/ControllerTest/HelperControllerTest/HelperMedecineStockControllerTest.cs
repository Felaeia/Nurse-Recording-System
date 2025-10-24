using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.Contracts.ServiceContracts.HelperContracts.IHelperUserForm;
using NurseRecordingSystem.DTO.HelperServiceDTOs.HelperMedecineStockDTOs;
using Xunit;

namespace NurseRecordingSystem.Test.ControllerTest.HelperControllerTest
{
    public class HelperMedecineStockControllerTest
    {
        private readonly Mock<IViewAllMedecineStocks> _mockViewService;
        private readonly HelperMedecineStockController _controller;

        public HelperMedecineStockControllerTest()
        {
            _mockViewService = new Mock<IViewAllMedecineStocks>();
            _controller = new HelperMedecineStockController(_mockViewService.Object);
        }

        [Fact]
        public async Task ViewAllStock_SuccessfulView_ReturnsOk()
        {
            // Arrange
            var expectedList = new List<ViewAllMedecineStockResponseDTO>
            {
                new ViewAllMedecineStockResponseDTO { MedicineId = 1, MedecineName = "Aspirin", MedecineDescription = "Pain reliever", NumberOfStock = 100 },
                new ViewAllMedecineStockResponseDTO { MedicineId = 2, MedecineName = "Ibuprofen", MedecineDescription = "Anti-inflammatory", NumberOfStock = 50 }
            };
            _mockViewService.Setup(service => service.ViewAllAsync()).ReturnsAsync(expectedList);

            // Act
            var result = await _controller.ViewAllStock() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedList, result.Value);
        }

        [Fact]
        public async Task ViewAllStock_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            _mockViewService.Setup(service => service.ViewAllAsync()).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.ViewAllStock() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Database error", result.Value?.ToString());
        }
    }
}
