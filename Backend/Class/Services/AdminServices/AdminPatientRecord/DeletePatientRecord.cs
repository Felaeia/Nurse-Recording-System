using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.IAdminServices.IAdminPatientRecords;

namespace NurseRecordingSystem.Class.Services.NurseServices.PatientRecords
{
    public class DeletePatientRecord : IDeletedPatientRecord
    {
        private readonly string? _connectionString;

        public DeletePatientRecord(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> SoftDeletePatientRecordAsync(int patientRecordId)
        {
            const string storedProc = "dbo.asp_DeletePatientRecord";
            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand(storedProc, connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@patientRecordId", patientRecordId);
                cmd.Parameters.AddWithValue("@deletedBy", "Admin");

                try
                {
                    await connection.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        // Record not found or already deleted
                        return false;
                    }
                    return true;
                }
                catch (SqlException sqlEx) when (sqlEx.Number == 50003) // Custom SP error number
                {
                    throw new KeyNotFoundException(sqlEx.Message, sqlEx);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error during soft deletion of patient record {patientRecordId}.", ex);
                }
            }
        }
    }
}