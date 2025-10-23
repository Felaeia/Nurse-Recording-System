using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseMedecineStock;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseMedecineStockDTOs;

namespace NurseRecordingSystem.Class.Services.MedecineStockServices
{
    public class UpdateMedecineStock : IUpdateMedecineStock
    {
        private readonly string? _connectionString;

        public UpdateMedecineStock(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<bool> UpdateAsync(int medicineId, UpdateMedecineStockRequestDTO request, string updatedBy)
        {
            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand("dbo.nsp_UpdateMedecineStock", connection)) // Using nsp_
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@medicineId", medicineId);
                cmd.Parameters.AddWithValue("@medecineName", request.MedecineName);
                cmd.Parameters.AddWithValue("@medecineDescription", request.MedecineDescription);
                cmd.Parameters.AddWithValue("@numberOfStock", request.NumberOfStock);
                cmd.Parameters.AddWithValue("@updatedBy", updatedBy);

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