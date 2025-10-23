using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.IAdminServices.IAdminClinicStatus;

namespace NurseRecordingSystem.Class.Services.ClinicStatusServices
{
    public class DeleteClinicStatus : IDeleteClinicStatus
    {
        private readonly string? _connectionString;

        public DeleteClinicStatus(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<bool> DeleteAsync(int logId, string deletedBy)
        {
            const string storedProc = "dbo.asp_DeleteClinicStatus"; // Using asp_
            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand(storedProc, connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@logId", logId);
                cmd.Parameters.AddWithValue("@deletedBy", deletedBy);

                var returnValue = cmd.Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int);
                returnValue.Direction = System.Data.ParameterDirection.ReturnValue;

                await connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return (int)returnValue.Value == 0; // 0 for success
            }
        }
    }
}