// File: NurseRecordingSystem.Class.Services.UserServices.UserForms/DeleteUserForm.cs

using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.UserForms;
using System.Data;

namespace NurseRecordingSystem.Class.Services.UserServices.UserForms
{
    public class DeleteUserForm : IDeleteUserForm
    {
        private readonly string? _connectionString;

        public DeleteUserForm(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<bool> DeleteUserFormAsync(int formId, string deletedBy)
        {
            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand("dbo.usp_DeleteUserForm", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Input Parameters
                cmd.Parameters.AddWithValue("@FormId", formId);

                //Chore: deletedBy should be the userId of the user performing the deletion
                cmd.Parameters.AddWithValue("@DeletedBy", deletedBy);

                // Output/Return Parameter from SP
                var returnValue = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;

                try
                {
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    // Retrieve the return code set by the stored procedure
                    int result = (int)returnValue.Value;

                    // The stored procedure returns 0 for success.
                    if (result == 0)
                    {
                        return true;
                    }
                    else if (result == 1)
                    {
                        // Specific error for "Form not found or already deleted"
                        throw new Exception($"Patient Form with ID {formId} not found or is already deleted.");
                    }
                    else
                    {
                        // Other database/update failure
                        return false;
                    }
                }
                catch (SqlException ex)
                {
                    // Catch specific SQL exceptions (e.g., connection issues)
                    throw new Exception("Database error occurred during user form deletion.", ex);
                }
                catch (Exception ex)
                {
                    // Catch other general errors
                    throw new Exception("An unexpected error occurred during user form deletion.", ex);
                }
            }
        }
    }
}