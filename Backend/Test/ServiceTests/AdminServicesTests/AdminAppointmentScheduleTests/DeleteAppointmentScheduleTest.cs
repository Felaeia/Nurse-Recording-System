using Moq;
using Xunit;
using NurseRecordingSystem.Class.Services.AdminServices.AdminAppointmentSchedule;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseAppointmentScheduleDTOs;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace NurseRecordingSystem.Test.ServiceTests.AdminServicesTests.AdminAppointmentScheduleTests
{
    public class DeleteAppointmentScheduleTest
    {
        [Fact]
        public void Constructor_NullDbExecutor_ThrowsArgumentNullException()
        {
            // Arrange
            IDbExecutor dbExecutor = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new DeleteAppointmentSchedule(dbExecutor));
            Assert.Contains("dbExecutor", exception.Message);
        }

        [Fact]
        public async Task DeleteAppointmentAsync_NullRequest_ThrowsArgumentNullException()
        {
            // Arrange
            var mockDbExecutor = new Mock<IDbExecutor>();
            var service = new DeleteAppointmentSchedule(mockDbExecutor.Object);
            DeleteAppointmentScheduleRequestDTO request = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.DeleteAppointmentAsync(1, request));
        }

        [Fact]
        public async Task DeleteAppointmentAsync_UnauthorizedNurse_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var mockDbExecutor = new Mock<IDbExecutor>();
            var service = new DeleteAppointmentSchedule(mockDbExecutor.Object);
            var request = new DeleteAppointmentScheduleRequestDTO { DeletedByNurseId = 1 };

            // Mock ExecuteAsync to set ResultCode to 1
            mockDbExecutor.Setup(db => db.ExecuteAsync(
                It.IsAny<string>(),
                It.IsAny<DynamicParameters>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>()))
                .Callback((string proc, object param, IDbTransaction trans, int? timeout, CommandType? type) =>
                {
                    var parameters = param as DynamicParameters;
                    if (parameters != null) parameters.Add("@ResultCode", 1, DbType.Int32, ParameterDirection.Output);
                })
                .ReturnsAsync(1);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => service.DeleteAppointmentAsync(1, request));
            Assert.IsType<UnauthorizedAccessException>(exception.InnerException);
            Assert.Contains("not authorized as a Nurse", exception.InnerException.Message);
        }

        [Fact]
        public async Task DeleteAppointmentAsync_AppointmentNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var mockDbExecutor = new Mock<IDbExecutor>();
            var service = new DeleteAppointmentSchedule(mockDbExecutor.Object);
            var request = new DeleteAppointmentScheduleRequestDTO { DeletedByNurseId = 1 };

            // Mock ExecuteAsync to set ResultCode to 2
            mockDbExecutor.Setup(db => db.ExecuteAsync(
                It.IsAny<string>(),
                It.IsAny<DynamicParameters>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>()))
                .Callback((string proc, object param, IDbTransaction trans, int? timeout, CommandType? type) =>
                {
                    var parameters = param as DynamicParameters;
                    if (parameters != null) parameters.Add("@ResultCode", 2, DbType.Int32, ParameterDirection.Output);
                })
                .ReturnsAsync(1);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => service.DeleteAppointmentAsync(1, request));
            Assert.IsType<KeyNotFoundException>(exception.InnerException);
            Assert.Contains("not found", exception.InnerException.Message);
        }

        [Fact]
        public async Task DeleteAppointmentAsync_DatabaseError_ThrowsException()
        {
            // Arrange
            var mockDbExecutor = new Mock<IDbExecutor>();
            var service = new DeleteAppointmentSchedule(mockDbExecutor.Object);
            var request = new DeleteAppointmentScheduleRequestDTO { DeletedByNurseId = 1 };

            // Mock ExecuteAsync to set ResultCode to -1
            mockDbExecutor.Setup(db => db.ExecuteAsync(
                It.IsAny<string>(),
                It.IsAny<DynamicParameters>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>()))
                .Callback((string proc, object param, IDbTransaction trans, int? timeout, CommandType? type) =>
                {
                    var parameters = param as DynamicParameters;
                    if (parameters != null) parameters.Add("@ResultCode", -1, DbType.Int32, ParameterDirection.Output);
                })
                .ReturnsAsync(1);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => service.DeleteAppointmentAsync(1, request));
            Assert.IsType<Exception>(exception.InnerException);
            Assert.Contains("unknown database error", exception.InnerException.Message);
        }

        [Fact]
        public async Task DeleteAppointmentAsync_ExecuteThrows_ThrowsApplicationException()
        {
            // Arrange
            var mockDbExecutor = new Mock<IDbExecutor>();
            var service = new DeleteAppointmentSchedule(mockDbExecutor.Object);
            var request = new DeleteAppointmentScheduleRequestDTO { DeletedByNurseId = 1 };

            // Mock ExecuteAsync to throw
            mockDbExecutor.Setup(db => db.ExecuteAsync(
                It.IsAny<string>(),
                It.IsAny<DynamicParameters>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>()))
                .ThrowsAsync(new Exception("Connection failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => service.DeleteAppointmentAsync(1, request));
            Assert.Contains("Service failed to soft-delete appointment", exception.Message);
        }

        [Fact]
        public async Task DeleteAppointmentAsync_Success_ReturnsTrue()
        {
            // Arrange
            var mockDbExecutor = new Mock<IDbExecutor>();
            var service = new DeleteAppointmentSchedule(mockDbExecutor.Object);
            var request = new DeleteAppointmentScheduleRequestDTO { DeletedByNurseId = 1 };

            // Mock ExecuteAsync to set ResultCode to 0
            mockDbExecutor.Setup(db => db.ExecuteAsync(
                It.IsAny<string>(),
                It.IsAny<DynamicParameters>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>()))
                .Callback((string proc, object param, IDbTransaction trans, int? timeout, CommandType? type) =>
                {
                    var parameters = param as DynamicParameters;
                    if (parameters != null) parameters.Add("@ResultCode", 0, DbType.Int32, ParameterDirection.Output);
                })
                .ReturnsAsync(1);

            // Act
            var result = await service.DeleteAppointmentAsync(1, request);

            // Assert
            Assert.True(result);
        }
    }
}
