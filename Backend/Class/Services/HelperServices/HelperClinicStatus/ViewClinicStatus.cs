using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.IHelperServices.IHelperClinicStatus;
using NurseRecordingSystem.DTO.HelperServiceDTOs.HelperClinicStatusDTOs;

namespace NurseRecordingSystem.Class.Services.ClinicStatusServices
{
    public class ViewClinicStatus : IViewClinicStatus
    {
        private readonly string? _connectionString;

        public ViewClinicStatus(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<List<ViewClinicStatusResponseDTO>> ViewAllAsync()
        {
            var statusList = new List<ViewClinicStatusResponseDTO>();
            const string storedProc = "dbo.hsp_ViewClinicStatus"; // Using hsp_

            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand(storedProc, connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                await connection.OpenAsync();

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        statusList.Add(new ViewClinicStatusResponseDTO
                        {
                            LogId = reader.GetInt32(reader.GetOrdinal("logId")),
                            Status = reader.GetBoolean(reader.GetOrdinal("status"))
                        });
                    }
                }
            }
            return statusList;
        }
    }
}