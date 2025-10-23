using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.HelperContracts.IHelperUserForm;
using NurseRecordingSystem.DTO.HelperServiceDTOs.HelperMedecineStockDTOs;

namespace NurseRecordingSystem.Class.Services.MedecineStockServices
{
    public class ViewAllMedecineStocks : IViewAllMedecineStocks
    {
        private readonly string? _connectionString;

        public ViewAllMedecineStocks(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<List<ViewAllMedecineStockResponseDTO>> ViewAllAsync()
        {
            var stockList = new List<ViewAllMedecineStockResponseDTO>();

            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand("dbo.hsp_ViewAllMedecineStock", connection)) // Using hsp_
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                await connection.OpenAsync();

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        stockList.Add(new ViewAllMedecineStockResponseDTO
                        {
                            MedicineId = reader.GetInt32(reader.GetOrdinal("medicineId")),
                            MedecineName = reader.GetString(reader.GetOrdinal("medecineName")),
                            MedecineDescription = reader.GetString(reader.GetOrdinal("medecineDescription")),
                            NumberOfStock = reader.GetInt32(reader.GetOrdinal("numberOfStock"))
                        });
                    }
                }
            }
            return stockList;
        }
    }
}