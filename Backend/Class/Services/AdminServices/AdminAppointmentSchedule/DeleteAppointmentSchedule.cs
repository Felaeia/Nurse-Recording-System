using Dapper;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseAppointmentScheduleDTOs;
using System.Data;

namespace NurseRecordingSystem.Class.Services.AdminServices.AdminAppointmentSchedule
{
    public interface IDbExecutor
    {
        Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
    }

    public class DapperDbExecutor : IDbExecutor
    {
        private readonly IDbConnection _dbConnection;

        public DapperDbExecutor(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _dbConnection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
        }
    }

    public class DeleteAppointmentSchedule : IDeleteAppointmentSchedule
    {
        private readonly IDbExecutor _dbExecutor;

        public DeleteAppointmentSchedule(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor ?? throw new ArgumentNullException(nameof(dbExecutor));
        }
        // I was testing dapper hahahahahah T.T
        public async Task<bool> DeleteAppointmentAsync(int appointmentId, DeleteAppointmentScheduleRequestDTO request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var parameters = new DynamicParameters();
            parameters.Add("@AppointmentId", appointmentId);
            parameters.Add("@NurseId", request.DeletedByNurseId, DbType.Int32);
            parameters.Add("@DeletedBy", request.DeletedByNurseId, DbType.Int32, size: 50);
            parameters.Add("@ResultCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

            try
            {
                // Execute the stored procedure
                await _dbExecutor.ExecuteAsync(
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
                    throw new KeyNotFoundException($"Appointment with ID {appointmentId} not found or is already deleted.");
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
