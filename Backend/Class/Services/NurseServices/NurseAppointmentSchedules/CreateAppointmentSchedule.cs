using Dapper;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseAppointmentScheduleDTOs;
using System.Data;

namespace NurseRecordingSystem.Class.Services.NurseServices.AppointmentSchedules
{
    public class CreateAppointmentSchedule : ICreateAppointmentSchedule
    {
        private readonly IDbConnection _dbConnection; // Mock DbContext/Connection

        public CreateAppointmentSchedule(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> CreateAppointmentAsync(CreateAppointmentScheduleRequestDTO request)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AppointmentTime", request.AppointmentTime, DbType.DateTime);
            parameters.Add("@AppointmentDescription", request.AppointmentDescription, DbType.String);
            parameters.Add("@NurseId", request.NurseId, DbType.Int32);
            parameters.Add("@CreatedBy", request.CreatedBy, DbType.String, size: 50);
            parameters.Add("@ResultCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            try
            {
                // Execute the stored procedure
                await _dbConnection.ExecuteAsync(
                    "nsp_CreateAppointmentSchedule",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                int resultCode = parameters.Get<int>("@ResultCode");

                if (resultCode == 1)
                {
                    // Error Handling: Not a Nurse
                    throw new UnauthorizedAccessException("User is not authorized as a Nurse to create an appointment.");
                }
                else if (resultCode == -1)
                {
                    // Error Handling: General DB Error
                    throw new Exception("An unknown database error occurred while creating the appointment.");
                }

                return resultCode == 0;
            }
            catch (Exception ex)
            {
                // Log the error
                throw new ApplicationException("Service failed to create appointment.", ex);
            }
        }
    }
}
