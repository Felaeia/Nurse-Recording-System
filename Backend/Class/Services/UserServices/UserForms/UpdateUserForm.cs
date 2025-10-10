using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.IUserForms;
using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UserFormsDTOs;
using System.Data;


namespace NurseRecordingSystem.Class.Services.UserServices.UserForms
{
    public class UpdateUserForm : IUpdateUserForm
    {
        private readonly string? _connectionString;

        public UpdateUserForm(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<UserFormResponseDTO> UpdateUserFormAsync(UserFormRequestDTO userFormRequest)
        {
            if (userFormRequest == null)
            {
                throw new ArgumentNullException(nameof(userFormRequest), "UserFormRequest Cannot be Null");
            }

            // We only need the fields relevant to the update.
            // Note: The SP only takes FormId, data fields, and UpdatedBy.
            string updatedBy = userFormRequest.updatedBy;

            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand("dbo.usp_UpdateUserForm", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Input Parameters based on SP definition
                cmd.Parameters.AddWithValue("@FormId", userFormRequest.formId);
                cmd.Parameters.AddWithValue("@IssueType", userFormRequest.issueType);
                cmd.Parameters.AddWithValue("@IssueDescryption", userFormRequest.issueDescryption);
                cmd.Parameters.AddWithValue("@Status", userFormRequest.status);
                cmd.Parameters.AddWithValue("@PatientName", userFormRequest.patientName);
                cmd.Parameters.AddWithValue("@UpdatedBy", updatedBy);

                // Output/Return Parameter from SP
                var returnCode = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
                returnCode.Direction = ParameterDirection.Output;

                try
                {
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    int result = (int)returnCode.Value;

                    if (result == 0) // Success
                    {
                        return new UserFormResponseDTO
                        {
                            IsSuccess = true,
                            UserFormId = userFormRequest.formId,
                            Message = $"Patient Form ID {userFormRequest.formId} updated successfully."
                        };
                    }
                    else if (result == 1) // Not Found
                    {
                        return new UserFormResponseDTO
                        {
                            IsSuccess = false,
                            UserFormId = userFormRequest.formId,
                            Message = $"Patient Form ID {userFormRequest.formId} not found or is inactive."
                        };
                    }
                    else // General Update Failure
                    {
                        return new UserFormResponseDTO
                        {
                            IsSuccess = false,
                            UserFormId = userFormRequest.formId,
                            Message = $"Update failed for Patient Form ID {userFormRequest.formId} (Error Code: {result})."
                        };
                    }
                }
                catch (Exception ex)
                {
                    // Catch-all for database connection or unexpected errors
                    throw new Exception($"An error occurred while updating patient form ID {userFormRequest.formId}.", ex);
                }
            }
        }
    }
}