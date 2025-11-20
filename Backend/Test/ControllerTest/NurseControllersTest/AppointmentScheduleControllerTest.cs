using Microsoft.AspNetCore.Mvc;
using Moq;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices;
using NurseRecordingSystem.Controllers.NurseControllers;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseAppointmentScheduleDTOs;
using Xunit;

namespace NurseRecordingSystem.Test.ControllerTest.NurseControllersTest
{
    public class AppointmentScheduleControllerTest
    {
        private readonly Mock<ICreateAppointmentSchedule> _mockCreateService;
        private readonly Mock<IViewAppointmentScheduleList> _mockViewListService;
        private readonly Mock<IViewAppointmentSchedule> _mockViewService;
        private readonly Mock<IUpdateAppointmentSchedule> _mockUpdateService;
        private readonly Mock<IDeleteAppointmentSchedule> _mockDeleteService;
        private readonly NurseAppointmentScheduleController _controller;

        public AppointmentScheduleControllerTest()
        {
            _mockCreateService = new Mock<ICreateAppointmentSchedule>();
            _mockViewListService = new Mock<IViewAppointmentScheduleList>();
            _mockViewService = new Mock<IViewAppointmentSchedule>();
            _mockUpdateService = new Mock<IUpdateAppointmentSchedule>();
            _mockDeleteService = new Mock<IDeleteAppointmentSchedule>();
            _controller = new NurseAppointmentScheduleController(
                _mockCreateService.Object,
                _mockViewListService.Object,
                _mockViewService.Object,
                _mockUpdateService.Object,
                _mockDeleteService.Object);
        }

        [Fact]
        public async Task CreateAppointment_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var request = new CreateAppointmentScheduleRequestDTO
            {
                AppointmentTime = DateTime.Now.AddDays(1),
                AppointmentDescription = "Checkup",
                NurseId = 1,
                CreatedBy = "Nurse1"
            };
            _mockCreateService.Setup(service => service.CreateAppointmentAsync(request)).ReturnsAsync(true);

            // Act
            var result = await _controller.CreateAppointment(request) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Contains("Appointment created successfully.", result.Value?.ToString());
        }

        [Fact]
        public async Task CreateAppointment_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateAppointmentScheduleRequestDTO(); 
            _controller.ModelState.AddModelError("AppointmentTime", "Required");

            // Act
            var result = await _controller.CreateAppointment(request) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task CreateAppointment_Unauthorized_ReturnsForbidden()
        {
            // Arrange
            var request = new CreateAppointmentScheduleRequestDTO
            {
                AppointmentTime = DateTime.Now.AddDays(1),
                AppointmentDescription = "Checkup",
                NurseId = 1,
                CreatedBy = "Nurse1"
            };
            _mockCreateService.Setup(service => service.CreateAppointmentAsync(request)).ThrowsAsync(new UnauthorizedAccessException("Unauthorized"));

            // Act
            var result = await _controller.CreateAppointment(request) as ForbidResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ViewAppointmentScheduleList_Success_ReturnsOk()
        {
            // Arrange
            var appointments = new List<ViewAppointmentScheduleListResponseDTO>
            {
                new ViewAppointmentScheduleListResponseDTO { AppointmentId = 1, AppointmentTime = DateTime.Now, AppointmentDescription = "Checkup" }
            };
            _mockViewListService.Setup(service => service.ViewAppointmentScheduleListAsync()).ReturnsAsync(appointments);

            // Act
            var result = await _controller.ViewAppointmentScheduleList() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(appointments, result.Value);
        }

        [Fact]
        public async Task ViewAppointmentSchedule_Success_ReturnsOk()
        {
            // Arrange
            var appointmentId = 1;
            var appointment = new ViewAppointmentScheduleResponseDTO
            {
                AppointmentId = appointmentId,
                AppointmentTime = DateTime.Now,
                AppointmentDescription = "Checkup",
                NurseId = 1,
                CreatedOn = DateTime.Now
            };
            _mockViewService.Setup(service => service.ViewAppointmentScheduleAsync(appointmentId)).ReturnsAsync(appointment);

            // Act
            var result = await _controller.ViewAppointmentSchedule(appointmentId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(appointment, result.Value);
        }

        [Fact]
        public async Task ViewAppointmentSchedule_NotFound_ReturnsNotFound()
        {
            // Arrange
            var appointmentId = 999;
            _mockViewService.Setup(service => service.ViewAppointmentScheduleAsync(appointmentId)).ThrowsAsync(new KeyNotFoundException("Appointment not found"));

            // Act
            var result = await _controller.ViewAppointmentSchedule(appointmentId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task UpdateAppointment_ValidRequest_ReturnsOk()
        {
            // Arrange
            var appointmentId = 1;
            var request = new UpdateAppointmentScheduleRequestDTO
            {
                AppointmentTime = DateTime.Now.AddDays(2),
                AppointmentDescription = "Updated Checkup",
                NurseId = 1,
                UpdatedBy = "Nurse1"
            };
            _mockUpdateService.Setup(service => service.UpdateAppointmentAsync(appointmentId, request)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateAppointment(appointmentId, request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Contains("Appointment updated successfully.", result.Value?.ToString());
        }

        [Fact]
        public async Task UpdateAppointment_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var appointmentId = 1;
            var request = new UpdateAppointmentScheduleRequestDTO(); // Invalid
            _controller.ModelState.AddModelError("AppointmentTime", "Required");

            // Act
            var result = await _controller.UpdateAppointment(appointmentId, request) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task UpdateAppointment_NotFound_ReturnsNotFound()
        {
            // Arrange
            var appointmentId = 999;
            var request = new UpdateAppointmentScheduleRequestDTO
            {
                AppointmentTime = DateTime.Now.AddDays(2),
                AppointmentDescription = "Updated Checkup",
                NurseId = 1,
                UpdatedBy = "Nurse1"
            };
            _mockUpdateService.Setup(service => service.UpdateAppointmentAsync(appointmentId, request)).ThrowsAsync(new KeyNotFoundException("Appointment not found"));

            // Act
            var result = await _controller.UpdateAppointment(appointmentId, request) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task DeleteAppointment_ValidRequest_ReturnsOk()
        {
            // Arrange
            var appointmentId = 1;
            var request = new DeleteAppointmentScheduleRequestDTO
            {
                DeletedByNurseId = 1
            };
            _mockDeleteService.Setup(service => service.DeleteAppointmentAsync(appointmentId, request)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteAppointment(appointmentId, request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Contains("Appointment deleted successfully (soft-deleted).", result.Value?.ToString());
        }

        [Fact]
        public async Task DeleteAppointment_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var appointmentId = 1;
            var request = new DeleteAppointmentScheduleRequestDTO(); // Invalid
            _controller.ModelState.AddModelError("DeletedByNurseId", "Required");

            // Act
            var result = await _controller.DeleteAppointment(appointmentId, request) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task DeleteAppointment_NotFound_ReturnsNotFound()
        {
            // Arrange
            var appointmentId = 999;
            var request = new DeleteAppointmentScheduleRequestDTO
            {
                DeletedByNurseId = 1
            };
            _mockDeleteService.Setup(service => service.DeleteAppointmentAsync(appointmentId, request)).ThrowsAsync(new KeyNotFoundException("Appointment not found"));

            // Act
            var result = await _controller.DeleteAppointment(appointmentId, request) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }
    }
}
