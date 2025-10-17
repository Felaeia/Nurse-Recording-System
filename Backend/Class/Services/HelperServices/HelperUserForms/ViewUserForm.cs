using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.HelperContracts.IHelperUserForm;

namespace NurseRecordingSystem.Class.Services.UserServices.UserForms
{
    // Assuming this implements IViewUserForm
    public class ViewUserForm : IViewUserForm
    {
        private readonly string? _connectionString;

        public ViewUserForm(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<ViewUserFormResponseDTO> GetUserFormAsync(int formId)
        {
            const string storedProc = "dbo.hsp_ViewUserForm";
            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand(storedProc, connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@formId", formId);

                try
                {
                    await connection.OpenAsync();
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Map the data to the comprehensive DTO
                            return new ViewUserFormResponseDTO
                            {
                                FormId = reader.GetInt32(reader.GetOrdinal("formId")),
                                IssueType = reader.GetString(reader.GetOrdinal("issueType")),
                                IssueDescryption = reader.IsDBNull(reader.GetOrdinal("issueDescryption")) ? null : reader.GetString(reader.GetOrdinal("issueDescryption")),
                                Status = reader.GetString(reader.GetOrdinal("status")),
                                UserId = reader.GetInt32(reader.GetOrdinal("userId")),
                                PatientName = reader.GetString(reader.GetOrdinal("patientName")),
                                CreatedOn = reader.IsDBNull(reader.GetOrdinal("createdOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("createdOn")),
                                CreatedBy = reader.IsDBNull(reader.GetOrdinal("createdBy")) ? null : reader.GetString(reader.GetOrdinal("createdBy")),
                                UpdatedOn = reader.IsDBNull(reader.GetOrdinal("updatedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updatedOn")),
                                UpdatedBy = reader.IsDBNull(reader.GetOrdinal("updatedBy")) ? null : reader.GetString(reader.GetOrdinal("updatedBy")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("isActive"))
                            };
                        }
                        else
                        {
                            // This case is handled by the SP's THROW 50001, but kept for safety.
                            throw new Exception($"Patient form with ID {formId} not found.");
                        }
                    }
                }
                catch (SqlException sqlEx) when (sqlEx.Number == 50001)
                {
                    // Catch the custom error thrown by the SP when the form is not found/archived
                    throw new KeyNotFoundException(sqlEx.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while retrieving the patient form.", ex);
                }
            }
        }
    }
}