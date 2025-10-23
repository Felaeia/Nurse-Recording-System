using Dapper;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseAppointmentScheduleDTOs;
using System.Data;

namespace NurseRecordingSystem.Class.Services.NurseServices.NurseAppointmentSchedules
{
    public class ViewAppointmentSchedule : IViewAppointmentSchedule
    {
        private readonly IDbConnection _dbConnection;

        public ViewAppointmentSchedule(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ViewAppointmentScheduleResponseDTO> ViewAppointmentScheduleAsync(int appointmentId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AppointmentId", appointmentId, DbType.Int32);

            try
            {
                // Execute the stored procedure
                var result = await _dbConnection.QuerySingleOrDefaultAsync<ViewAppointmentScheduleResponseDTO>(
                    "nsp_ViewAppointmentSchedule",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (result == null)
                {
                    // Error Handling: Appointment not found
                    throw new KeyNotFoundException($"Appointment with ID {appointmentId} not found or is inactive.");
                }

                return result;
            }
            catch (Exception ex)
            {
                // Log the error
                throw new ApplicationException($"Service failed to retrieve appointment {appointmentId}.", ex);
            }
        }
    }
}