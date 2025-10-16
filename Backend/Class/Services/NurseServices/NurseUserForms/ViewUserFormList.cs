using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseUserForms;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.UserForms; // Assuming interface IViewUserFormList
using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UserFormsDTOs;
using System.Collections.Generic;

namespace NurseRecordingSystem.Class.Services.UserServices.UserForms
{
    // Assuming this implements IViewUserFormList
    public class ViewUserFormList : IViewUserFormList
    {
        private readonly string? _connectionString;

        public ViewUserFormList(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        // Returns a list of the simplified DTO
        public async Task<List<UserFormListItemDTO>> GetUserFormListAsync(int? userId = null, string? status = null)
        {
            var forms = new List<UserFormListItemDTO>();
            const string storedProc = "dbo.nsp_ViewUserFormList";

            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand(storedProc, connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Add optional parameters, using DBNull.Value if null
                cmd.Parameters.AddWithValue("@userId", userId.HasValue ? (object)userId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@status", status ?? (object)DBNull.Value);

                try
                {
                    await connection.OpenAsync();
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // Map the data to the list item DTO
                            forms.Add(new UserFormListItemDTO
                            {
                                FormId = reader.GetInt32(reader.GetOrdinal("formId")),
                                IssueType = reader.GetString(reader.GetOrdinal("issueType")),
                                Status = reader.GetString(reader.GetOrdinal("status")),
                                PatientName = reader.GetString(reader.GetOrdinal("patientName"))
                            });
                        }
                    }
                    return forms;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while retrieving the list of patient forms.", ex);
                }
            }
        }
    }
}