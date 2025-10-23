using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseMedecineStock;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseMedecineStockDTOs;

namespace NurseRecordingSystem.Class.Services.MedecineStockServices
{
    public class CreateMedecineStock : ICreateMedecineStock
    {
        private readonly string? _connectionString;

        public CreateMedecineStock(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<int> CreateMedecineStockAsync(CreateMedecineStockRequestDTO request, string createdBy)
        {
            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand("dbo.asp_CreateMedecineStock", connection)) // Using asp_
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@medecineName", request.MedecineName);
                cmd.Parameters.AddWithValue("@medecineDescription", request.MedecineDescription);
                cmd.Parameters.AddWithValue("@numberOfStock", request.NumberOfStock);
                cmd.Parameters.AddWithValue("@nurseId", request.NurseId);
                cmd.Parameters.AddWithValue("@createdBy", createdBy);

                await connection.OpenAsync();
                var result = await cmd.ExecuteScalarAsync(); // Gets the NewMedicineId

                if (result == null || result is DBNull)
                {
                    throw new Exception("Failed to retrieve ID after creating medicine stock.");
                }

                return Convert.ToInt32(result);
            }
        }
    }
}