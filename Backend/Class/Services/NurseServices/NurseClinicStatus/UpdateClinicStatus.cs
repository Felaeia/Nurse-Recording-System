using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseClinicStatus;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseClinicStatusDTOs;

namespace NurseRecordingSystem.Class.Services.ClinicStatusServices
{
    public class UpdateClinicStatus : IUpdateClinicStatus
    {
        private readonly string? _connectionString;

        public UpdateClinicStatus(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<bool> UpdateAsync(int logId, UpdateClinicStatusRequestDTO request, string updatedBy)
        {
            const string storedProc = "dbo.nsp_UpdateClinicStatus"; // Using nsp_
            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand(storedProc, connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@logId", logId);
                cmd.Parameters.AddWithValue("@status", request.Status);
                cmd.Parameters.AddWithValue("@updatedBy", updatedBy);

                var returnValue = cmd.Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int);
                returnValue.Direction = System.Data.ParameterDirection.ReturnValue;

                await connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return (int)returnValue.Value == 0; // 0 for success
            }
        }
    }
}