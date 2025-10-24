using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseMedecineStock;
using NurseRecordingSystem.Controllers.NurseControllers;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseMedecineStockDTOs;
using System.Security.Claims;
using Xunit;

namespace NurseRecordingSystem.Test.ControllerTest.NurseControllersTest
{
    public class NurseMedecineStockControllerTest
    {
        private readonly Mock<ICreateMedecineStock> _mockCreateService;
        private readonly Mock<IDeleteMedecineStock> _mockDeleteService;
        private readonly Mock<IUpdateMedecineStock> _mockUpdateService;
        private readonly NurseMedecineStockController _controller;

        public NurseMedecineStockControllerTest()
        {
            _mockCreateService = new Mock<ICreateMedecineStock>();
            _mockDeleteService = new Mock<IDeleteMedecineStock>();
            _mockUpdateService = new Mock<IUpdateMedecineStock>();
            _controller = new NurseMedecineStockController(_mockCreateService.Object, _mockDeleteService.Object, _mockUpdateService.Object);
        }

        [Fact]
        public async Task CreateStock_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var request = new CreateMedecineStockRequestDTO
            {
                MedecineName = "Paracetamol",
                MedecineDescription = "Pain reliever",
                NumberOfStock = 100,
                NurseId = 1
            };
            var createdById = "AdminUser";
            var newId = 1;

            // Mock the user identity
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, createdById) };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };

            _mockCreateService.Setup(service => service.CreateMedecineStockAsync(request, createdById)).ReturnsAsync(newId);

            // Act
            var result = await _controller.CreateStock(request) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(newId, ((dynamic)result.Value).MedicineId);
        }

        [Fact]
        public async Task CreateStock_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateMedecineStockRequestDTO(); // Invalid as required fields are empty
            _controller.ModelState.AddModelError("MedecineName", "Required");

            // Act
            var result = await _controller.CreateStock(request) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task CreateStock_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var request = new CreateMedecineStockRequestDTO
            {
                MedecineName = "Paracetamol",
                MedecineDescription = "Pain reliever",
                NumberOfStock = 100,
                NurseId = 1
            };
            var createdById = "AdminUser";

            _mockCreateService.Setup(service => service.CreateMedecineStockAsync(request, createdById)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.CreateStock(request) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Internal server error: Database error", result.Value?.ToString());
        }

        [Fact]
        public async Task DeleteStock_ValidRequest_ReturnsNoContent()
        {
            // Arrange
            var id = 1;
            var deletedBy = "AdminUser";

            // Mock the user identity
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, deletedBy) };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };

            _mockDeleteService.Setup(service => service.DeleteMedecineStockAsync(id, deletedBy)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteStock(id) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task DeleteStock_NotFound_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var deletedBy = "AdminUser";

            _mockDeleteService.Setup(service => service.DeleteMedecineStockAsync(id, deletedBy)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteStock(id) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Contains($"Medicine Stock with ID {id} not found or is inactive.", result.Value?.ToString());
        }

        [Fact]
        public async Task DeleteStock_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var id = 1;
            var deletedBy = "AdminUser";

            _mockDeleteService.Setup(service => service.DeleteMedecineStockAsync(id, deletedBy)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.DeleteStock(id) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Internal server error: Database error", result.Value?.ToString());
        }

        [Fact]
        public async Task UpdateStock_ValidRequest_ReturnsNoContent()
        {
            // Arrange
            var id = 1;
            var request = new UpdateMedecineStockRequestDTO
            {
                MedecineName = "Updated Paracetamol",
                MedecineDescription = "Updated description",
                NumberOfStock = 150
            };
            var updatedBy = "NurseUser";

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
            var result = await _controller.UpdateStock(id, request) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task UpdateStock_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var id = 1;
            var request = new UpdateMedecineStockRequestDTO(); // Invalid as required fields are empty
            _controller.ModelState.AddModelError("MedecineName", "Required");

            // Act
            var result = await _controller.UpdateStock(id, request) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task UpdateStock_NotFound_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var request = new UpdateMedecineStockRequestDTO
            {
                MedecineName = "Updated Paracetamol",
                MedecineDescription = "Updated description",
                NumberOfStock = 150
            };
            var updatedBy = "NurseUser";

            _mockUpdateService.Setup(service => service.UpdateAsync(id, request, updatedBy)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateStock(id, request) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Contains($"Medicine Stock with ID {id} not found or is inactive.", result.Value?.ToString());
        }

        [Fact]
        public async Task UpdateStock_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var id = 1;
            var request = new UpdateMedecineStockRequestDTO
            {
                MedecineName = "Updated Paracetamol",
                MedecineDescription = "Updated description",
                NumberOfStock = 150
            };
            var updatedBy = "NurseUser";

            _mockUpdateService.Setup(service => service.UpdateAsync(id, request, updatedBy)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.UpdateStock(id, request) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Internal server error: Database error", result.Value?.ToString());
        }
    }
}
