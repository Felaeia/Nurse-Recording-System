using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.API.Controllers;
using NurseRecordingSystem.Contracts.ServiceContracts.IAdminServices.IAdminPatientRecords;
using Xunit;

namespace NurseRecordingSystem.Test.ControllerTest.AdminControllerTest
{
    public class AdminPatientRecordControllerTest
    {
        private readonly Mock<IDeletedPatientRecord> _mockDeleteService;
        private readonly AdminPatientRecordController _controller;

        public AdminPatientRecordControllerTest()
        {
            _mockDeleteService = new Mock<IDeletedPatientRecord>();
            _controller = new AdminPatientRecordController(_mockDeleteService.Object);
        }

        [Fact]
        public async Task DeleteRecord_SuccessfulDeletion_ReturnsNoContent()
        {
            // Arrange
            var patientRecordId = 123;
            _mockDeleteService.Setup(IDeletedPatientRecord => IDeletedPatientRecord.SoftDeletePatientRecordAsync(patientRecordId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteRecord(patientRecordId) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task DeleteRecord_NotFound_ReturnsNotFound()
        {
            // Arrange
            var patientRecordId = 999;
            _mockDeleteService.Setup(IDeletedPatientRecord => IDeletedPatientRecord.SoftDeletePatientRecordAsync(patientRecordId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteRecord(patientRecordId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Contains("not found or is already deleted", result.Value?.ToString());
        }

        [Fact]
        public async Task DeleteRecord_KeyNotFoundException_ReturnsNotFound()
        {
            // Arrange
            var patientRecordId = 123;
            _mockDeleteService.Setup(IDeletedPatientRecord => IDeletedPatientRecord.SoftDeletePatientRecordAsync(patientRecordId)).ThrowsAsync(new KeyNotFoundException("Record not found"));

            // Act
            var result = await _controller.DeleteRecord(patientRecordId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task DeleteRecord_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var patientRecordId = 123;
            _mockDeleteService.Setup(IDeletedPatientRecord => IDeletedPatientRecord.SoftDeletePatientRecordAsync(patientRecordId)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.DeleteRecord(patientRecordId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Database error", result.Value?.ToString());
        }
    }
}
