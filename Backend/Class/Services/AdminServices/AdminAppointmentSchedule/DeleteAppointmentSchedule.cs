using Dapper;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseAppointmentScheduleDTOs;
using System.Data;

namespace NurseRecordingSystem.Class.Services.AdminServices.AdminAppointmentSchedule
{
    public class DeleteAppointmentSchedule : IDeleteAppointmentSchedule
    {
        private readonly IDbConnection _dbConnection;

        public DeleteAppointmentSchedule(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> DeleteAppointmentAsync(int appointmentId, DeleteAppointmentScheduleRequestDTO request)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AppointmentId", appointmentId);
            parameters.Add("@NurseId", request.NurseId, DbType.Int32);
            parameters.Add("@DeletedBy", request.DeletedBy, DbType.String, size: 50);
            parameters.Add("@ResultCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            try
            {
                // Execute the stored procedure
                await _dbConnection.ExecuteAsync(
                    "nsp_DeleteAppointmentSchedule",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                int resultCode = parameters.Get<int>("@ResultCode");

                if (resultCode == 1)
                {
                    throw new UnauthorizedAccessException("User is not authorized as a Nurse to delete an appointment.");
                }
                else if (resultCode == 2)
                {
                    throw new KeyNotFoundException($"Appointment with ID {request.AppointmentId} not found or is already deleted.");
                }
                else if (resultCode == -1)
                {
                    throw new Exception("An unknown database error occurred while deleting the appointment.");
                }

                return resultCode == 0;
            }
            catch (Exception ex)
            {
                // Log the error
                throw new ApplicationException("Service failed to soft-delete appointment.", ex);
            }
        }
    }
}
