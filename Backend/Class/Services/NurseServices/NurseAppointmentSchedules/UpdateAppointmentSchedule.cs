using Dapper;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseAppointmentScheduleDTOs;
using System.Data;

namespace NurseRecordingSystem.Class.Services.NurseServices.AppointmentSchedules
{
    public class UpdateAppointmentSchedule : IUpdateAppointmentSchedule
    {
        private readonly IDbConnection _dbConnection;

        public UpdateAppointmentSchedule(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> UpdateAppointmentAsync(int appointmentId,UpdateAppointmentScheduleRequestDTO request)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AppointmentId", appointmentId);
            parameters.Add("@AppointmentTime", request.AppointmentTime, DbType.DateTime);
            parameters.Add("@PatientName", request.PatientName, DbType.String);
            parameters.Add("@AppointmentDescription", request.AppointmentDescription, DbType.String);
            parameters.Add("@NurseId", request.NurseId, DbType.Int32);
            parameters.Add("@UpdatedBy", request.UpdatedBy, DbType.String, size: 50);
            parameters.Add("@ResultCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            try
            {
                // Execute the stored procedure
                await _dbConnection.ExecuteAsync(
                    "nsp_UpdateAppointmentSchedule",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                int resultCode = parameters.Get<int>("@ResultCode");

                if (resultCode == 1)
                {
                    throw new UnauthorizedAccessException("User is not authorized as a Nurse to update an appointment.");
                }
                else if (resultCode == 2)
                {
                    throw new KeyNotFoundException($"Appointment with ID {appointmentId} not found or is inactive.");
                }
                else if (resultCode == -1)
                {
                    throw new Exception("An unknown database error occurred while updating the appointment.");
                }

                return resultCode == 0;
            }
            catch (Exception ex)
            {
                // Log the error
                throw new ApplicationException("Service failed to update appointment.", ex);
            }
        }
    }
}
