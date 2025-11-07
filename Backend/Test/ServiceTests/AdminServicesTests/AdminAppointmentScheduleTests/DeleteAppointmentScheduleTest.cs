
using Xunit;

using Moq;

using Dapper;

using System.Data;

using System.Reflection;

using NurseRecordingSystem.Class.Services.AdminServices.AdminAppointmentSchedule;

using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseAppointmentScheduleDTOs;


namespace NurseRecordingSystem.Test.ServiceTests.AdminServicesTests.AdminAppointmentScheduleTests
{
    public class DeleteAppointmentScheduleTest
    {
        /// <summary>
        /// Tests that DeleteAppointmentAsync returns true when the stored procedure executes successfully (resultCode 0).
        /// Verifies that the method calls the database with correct parameters and returns the expected result.
        /// </summary>
        [Fact]
        public async Task DeleteAppointmentAsync_Success_ReturnsTrue()
        {
            // Arrange
            var mockDb = new Mock<IDbConnection>();
            var service = new DeleteAppointmentSchedule(mockDb.Object);
            var request = new DeleteAppointmentScheduleRequestDTO { DeletedByNurseId = 1 };
            int appointmentId = 123;

            mockDb.Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), It.IsAny<int?>(), It.IsAny<CommandType?>()))
                .Callback<string, object, IDbTransaction, int?, CommandType?>((sp, param, trans, timeout, ct) =>
                {
                    var parameters = param as DynamicParameters;
                    if (parameters != null)
                    {
                        var field = typeof(DynamicParameters).GetField("parameters", BindingFlags.NonPublic | BindingFlags.Instance);
                        var dict = field?.GetValue(parameters) as Dictionary<string, object>;
                        if (dict != null) dict["@ResultCode"] = 0;
                    }
                })
                .ReturnsAsync(1);

            // Act
            var result = await service.DeleteAppointmentAsync(appointmentId, request);

            // Assert
            Assert.True(result);
            mockDb.Verify(x => x.ExecuteAsync("nsp_DeleteAppointmentSchedule", It.Is<DynamicParameters>(p =>
                (int)p.Get<object>("@AppointmentId") == appointmentId &&
                (int)p.Get<object>("@NurseId") == 1 &&
                (int)p.Get<object>("@DeletedBy") == 1
            ), null, null, CommandType.StoredProcedure), Times.Once);
        }

        /// <summary>
        /// Tests that DeleteAppointmentAsync throws UnauthorizedAccessException when the stored procedure returns resultCode 1 (unauthorized).
        /// Verifies the correct exception type and message.
        /// </summary>
        [Fact]
        public async Task DeleteAppointmentAsync_Unauthorized_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var mockDb = new Mock<IDbConnection>();
            var service = new DeleteAppointmentSchedule(mockDb.Object);
            var request = new DeleteAppointmentScheduleRequestDTO { DeletedByNurseId = 1 };
            int appointmentId = 123;

            mockDb.Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), It.IsAny<int?>(), It.IsAny<CommandType?>()))
                .Callback<string, object, IDbTransaction, int?, CommandType?>((sp, param, trans, timeout, ct) =>
                {
                    var parameters = param as DynamicParameters;
                    if (parameters != null)
                    {
                        var field = typeof(DynamicParameters).GetField("parameters", BindingFlags.NonPublic | BindingFlags.Instance);
                        var dict = field?.GetValue(parameters) as Dictionary<string, object>;
                        if (dict != null) dict["@ResultCode"] = 1;
                    }
                })
                .ReturnsAsync(1);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.DeleteAppointmentAsync(appointmentId, request));
            Assert.Equal("User is not authorized as a Nurse to delete an appointment.", exception.Message);
        }

        /// <summary>
        /// Tests that DeleteAppointmentAsync throws KeyNotFoundException when the stored procedure returns resultCode 2 (not found).
        /// Verifies the correct exception type and message.
        /// </summary>
        [Fact]
        public async Task DeleteAppointmentAsync_NotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var mockDb = new Mock<IDbConnection>();
            var service = new DeleteAppointmentSchedule(mockDb.Object);
            var request = new DeleteAppointmentScheduleRequestDTO { DeletedByNurseId = 1 };
            int appointmentId = 123;

            mockDb.Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), It.IsAny<int?>(), It.IsAny<CommandType?>()))
                .Callback<string, object, IDbTransaction, int?, CommandType?>((sp, param, trans, timeout, ct) =>
                {
                    var parameters = param as DynamicParameters;
                    if (parameters != null)
                    {
                        var field = typeof(DynamicParameters).GetField("parameters", BindingFlags.NonPublic | BindingFlags.Instance);
                        var dict = field?.GetValue(parameters) as Dictionary<string, object>;
                        if (dict != null) dict["@ResultCode"] = 2;
                    }
                })
                .ReturnsAsync(1);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteAppointmentAsync(appointmentId, request));
            Assert.Equal($"Appointment with ID {appointmentId} not found or is already deleted.", exception.Message);
        }

        /// <summary>
        /// Tests that DeleteAppointmentAsync throws Exception when the stored procedure returns resultCode -1 (database error).
        /// Verifies the correct exception type and message.
        /// </summary>
        [Fact]
        public async Task DeleteAppointmentAsync_DatabaseError_ThrowsException()
        {
            // Arrange
            var mockDb = new Mock<IDbConnection>();
            var service = new DeleteAppointmentSchedule(mockDb.Object);
            var request = new DeleteAppointmentScheduleRequestDTO { DeletedByNurseId = 1 };
            int appointmentId = 123;

            mockDb.Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), It.IsAny<int?>(), It.IsAny<CommandType?>()))
                .Callback<string, object, IDbTransaction, int?, CommandType?>((sp, param, trans, timeout, ct) =>
                {
                    var parameters = param as DynamicParameters;
                    if (parameters != null)
                    {
                        var field = typeof(DynamicParameters).GetField("parameters", BindingFlags.NonPublic | BindingFlags.Instance);
                        var dict = field?.GetValue(parameters) as Dictionary<string, object>;
                        if (dict != null) dict["@ResultCode"] = -1;
                    }
                })
                .ReturnsAsync(1);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => service.DeleteAppointmentAsync(appointmentId, request));
            Assert.Equal("An unknown database error occurred while deleting the appointment.", exception.Message);
        }

        /// <summary>
        /// Tests that DeleteAppointmentAsync throws ApplicationException when a general database exception occurs.
        /// Verifies the correct exception type, message, and inner exception.
        /// </summary>
        [Fact]
        public async Task DeleteAppointmentAsync_GeneralException_ThrowsApplicationException()
        {
            // Arrange
            var mockDb = new Mock<IDbConnection>();
            var service = new DeleteAppointmentSchedule(mockDb.Object);
            var request = new DeleteAppointmentScheduleRequestDTO { DeletedByNurseId = 1 };
            int appointmentId = 123;

            mockDb.Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), It.IsAny<int?>(), It.IsAny<CommandType?>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => service.DeleteAppointmentAsync(appointmentId, request));
            Assert.Equal("Service failed to soft-delete appointment.", exception.Message);
            Assert.IsType<Exception>(exception.InnerException);
        }
    }
}
