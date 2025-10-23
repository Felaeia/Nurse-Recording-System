using Dapper;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseAppointmentScheduleDTOs;
using System.Data;

namespace NurseRecordingSystem.Class.Services.NurseServices.AppointmentSchedules
{
    public class ViewAppointmentScheduleList : IViewAppointmentScheduleList
    {
        private readonly IDbConnection _dbConnection;

        public ViewAppointmentScheduleList(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<ViewAppointmentScheduleListResponseDTO>> ViewAppointmentScheduleListAsync()
        {
            try
            {
                // Execute the stored procedure
                var result = await _dbConnection.QueryAsync<ViewAppointmentScheduleListResponseDTO>(
                    "nsp_ViewAppointmentScheduleList",
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
            catch (Exception ex)
            {
                // Log the error
                throw new ApplicationException("Service failed to retrieve appointment list.", ex);
            }
        }
    }
}
