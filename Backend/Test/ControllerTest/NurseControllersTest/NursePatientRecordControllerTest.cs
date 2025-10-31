using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.API.Controllers;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INursePatientRecords;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NursePatientRecordDTOs;
using NurseRecordingSystem.Model.DTO.NurseServicesDTOs.PatientRecordsDTOs;
using System.Security.Claims;
using Xunit;

namespace NurseRecordingSystem.Test.ControllerTest.NurseControllersTest
{
    public class NursePatientRecordControllerTest
    {
        private readonly Mock<IViewPatientRecord> _mockViewService;
        private readonly Mock<IViewPatientRecordList> _mockViewListService;
        private readonly Mock<IUpdatePatientRecord> _mockUpdateService;
        private readonly Mock<ICreatePatientRecord> _mockCreateService;
        private readonly NursePatientRecordController _controller;

        public NursePatientRecordControllerTest()
        {
            _mockViewService = new Mock<IViewPatientRecord>();
            _mockViewListService = new Mock<IViewPatientRecordList>();
            _mockUpdateService = new Mock<IUpdatePatientRecord>();
            _mockCreateService = new Mock<ICreatePatientRecord>();
            _controller = new NursePatientRecordController(
                _mockViewService.Object,
                _mockViewListService.Object,
                _mockUpdateService.Object,
                _mockCreateService.Object);
        }

        [Fact]
        public async Task CreateRecord_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var request = new CreatePatientRecordRequestDTO { };
            var createdBy = "Nurse1";
            var newId = 1;

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["X-Created-By"] = createdBy;

            _mockCreateService.Setup(service => service.CreatePatientRecordAsync(request, createdBy)).ReturnsAsync(newId);

            // Act
            var result = await _controller.CreateRecord(request, createdBy) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(newId, result.Value);
        }

        [Fact]
        public async Task CreateRecord_MissingHeader_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreatePatientRecordRequestDTO { };
            var createdBy = "";

            // Act
            var result = await _controller.CreateRecord(request, createdBy) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("Audit header 'X-Created-By' is required.", result.Value?.ToString());
        }

        [Fact]
        public async Task CreateRecord_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreatePatientRecordRequestDTO { };
            var createdBy = "Nurse1";

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["X-Created-By"] = createdBy;

            _mockCreateService.Setup(service => service.CreatePatientRecordAsync(request, createdBy)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.CreateRecord(request, createdBy) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("Database error", result.Value?.ToString());
        }

        [Fact]
        public async Task GetRecord_ValidRequest_ReturnsOk()
        {
            // Arrange
            var patientRecordId = 1;
            var response = new ViewPatientRecordResponseDTO { };

            _mockViewService.Setup(service => service.GetPatientRecordAsync(patientRecordId)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetRecord(patientRecordId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task GetRecord_NotFound_ReturnsNotFound()
        {
            // Arrange
            var patientRecordId = 999;

            _mockViewService.Setup(service => service.GetPatientRecordAsync(patientRecordId)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.GetRecord(patientRecordId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetRecord_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var patientRecordId = 1;

            _mockViewService.Setup(service => service.GetPatientRecordAsync(patientRecordId)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetRecord(patientRecordId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Database error", result.Value?.ToString());
        }

        [Fact]
        public async Task GetRecordList_ValidRequest_ReturnsOk()
        {
            // Arrange
            var nurseId = 1;
            var records = new List<PatientRecordListItemDTO> { };

            _mockViewListService.Setup(service => service.GetPatientRecordListAsync(nurseId)).ReturnsAsync(records);

            // Act
            var result = await _controller.GetRecordList(nurseId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(records, result.Value);
        }

        [Fact]
        public async Task GetRecordList_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var nurseId = 1;

            _mockViewListService.Setup(service => service.GetPatientRecordListAsync(nurseId)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetRecordList(nurseId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Database error", result.Value?.ToString());
        }

        [Fact]
        public async Task UpdateRecord_ValidRequest_ReturnsOk()
        {
            // Arrange
            var patientRecordId = 1;
            var request = new UpdatePatientRecordRequestDTO { PatientRecordId = 1, };
            var updatedBy = "Nurse1";
            var response = new UpdatePatientRecordResponseDTO { };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["X-Updated-By"] = updatedBy;

            _mockUpdateService.Setup(service => service.UpdatePatientRecordAsync(request, updatedBy)).ReturnsAsync(response);

            // Act
            var result = await _controller.UpdateRecord(patientRecordId, request, updatedBy) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task UpdateRecord_IdMismatch_ReturnsBadRequest()
        {
            // Arrange
            var patientRecordId = 1;
            var request = new UpdatePatientRecordRequestDTO { PatientRecordId = 2, };
            var updatedBy = "Nurse1";

            // Act
            var result = await _controller.UpdateRecord(patientRecordId, request, updatedBy) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("ID mismatch between route and body.", result.Value?.ToString());
        }

        [Fact]
        public async Task UpdateRecord_MissingHeader_ReturnsBadRequest()
        {
            // Arrange
            var patientRecordId = 1;
            var request = new UpdatePatientRecordRequestDTO { PatientRecordId = 1, };
            var updatedBy = "";

            // Act
            var result = await _controller.UpdateRecord(patientRecordId, request, updatedBy) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("Audit header 'X-Updated-By' is required.", result.Value?.ToString());
        }

        [Fact]
        public async Task UpdateRecord_NotFound_ReturnsNotFound()
        {
            // Arrange
            var patientRecordId = 1;
            var request = new UpdatePatientRecordRequestDTO { PatientRecordId = 1, };
            var updatedBy = "Nurse1";

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["X-Updated-By"] = updatedBy;

            _mockUpdateService.Setup(service => service.UpdatePatientRecordAsync(request, updatedBy)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.UpdateRecord(patientRecordId, request, updatedBy) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Contains($"Record with ID {patientRecordId} not found or cannot be updated.", result.Value?.ToString());
        }

        [Fact]
        public async Task UpdateRecord_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var patientRecordId = 1;
            var request = new UpdatePatientRecordRequestDTO { PatientRecordId = 1, };
            var updatedBy = "Nurse1";

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["X-Updated-By"] = updatedBy;

            _mockUpdateService.Setup(service => service.UpdatePatientRecordAsync(request, updatedBy)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.UpdateRecord(patientRecordId, request, updatedBy) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Database error", result.Value?.ToString());
        }
    }
}
