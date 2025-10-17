using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.IAdminServices.IAdminClinicStatus;
using NurseRecordingSystem.DTO.AdminServiceDTOs.AdminClinicStatusDTOs;

namespace NurseRecordingSystem.Class.Services.ClinicStatusServices
{
    public class CreateClinicStatus : ICreateClinicStatus
    {
        private readonly string? _connectionString;

        public CreateClinicStatus(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<int> CreateAsync(CreateClinicStatusRequestDTO request, string createdBy)
        {
            const string storedProc = "dbo.asp_CreateClinicStatus"; // Using asp_
            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand(storedProc, connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@status", request.Status);
                cmd.Parameters.AddWithValue("@nurseId", request.NurseId);
                cmd.Parameters.AddWithValue("@createdBy", createdBy);
                // Note: updatedBy is required in the SP and is often the same as createdBy upon creation
                cmd.Parameters.AddWithValue("@updatedBy", createdBy);

                await connection.OpenAsync();
                var result = await cmd.ExecuteScalarAsync(); // Returns NewLogId

                if (result == null || result is DBNull)
                {
                    throw new Exception("Failed to retrieve ID after creating clinic status log.");
                }

                return Convert.ToInt32(result);
            }
        }
    }
}