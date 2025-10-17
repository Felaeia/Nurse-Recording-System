using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseMedecineStock;

namespace NurseRecordingSystem.Class.Services.MedecineStockServices
{
    public class DeleteMedecineStock : IDeleteMedecineStock
    {
        private readonly string? _connectionString;

        public DeleteMedecineStock(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<bool> DeleteMedecineStockAsync(int medicineId, string deletedBy)
        {
            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand("dbo.asp_DeleteMedecineStock", connection)) // Using asp_
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@medicineId", medicineId);
                cmd.Parameters.AddWithValue("@deletedBy", deletedBy);

                var returnValue = cmd.Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int);
                returnValue.Direction = System.Data.ParameterDirection.ReturnValue;

                await connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                // Stored procedure returns 0 for success, 1 for failure/not found
                return (int)returnValue.Value == 0;
            }
        }
    }
}