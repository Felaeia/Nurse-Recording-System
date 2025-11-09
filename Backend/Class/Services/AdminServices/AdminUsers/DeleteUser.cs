using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.IAdminServices.IAdminUser;

namespace NurseRecordingSystem.Class.Services.UserServices.Users
{
    public class DeleteUser : IDeleteUser
    {
        private readonly string? _connectionString;

        public DeleteUser(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        // Handles the business logic of deleting user (soft) based on the userId
        public async Task<bool> SoftDeleteUserAsync(int userId, string deletedBy)
        {
            if (string.IsNullOrEmpty(deletedBy))
            {
                throw new ArgumentNullException(nameof(deletedBy));
            }

            // The stored procedure to call
            const string storedProc = "dbo.asp_DeleteUser";
            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand(storedProc, connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Input parameters for the stored procedure
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@updatedBy", deletedBy);

                // Add an output parameter to capture the return code from the stored procedure
                var returnParameter = cmd.Parameters.Add("@ReturnVal", System.Data.SqlDbType.Int);
                returnParameter.Direction = System.Data.ParameterDirection.ReturnValue;

                try
                {
                    await connection.OpenAsync();

                    // ExecuteNonQuery is used for CUD operations (Create, Update, Delete)
                    await cmd.ExecuteNonQueryAsync();

                    var returnCode = (int)returnParameter.Value;

                    // Check for custom error codes from the stored procedure
                    if (returnCode != 0)
                    {
                        // Example handling for the custom error code 50001 from the SP
                        if (returnCode == 50001)
                        {
                            throw new Exception($"Soft delete failed: User with ID {userId} was not found.");
                        }
                        // Generic error for other custom codes
                        throw new Exception($"Soft delete failed with database return code: {returnCode}");
                    }

                    // Success
                    return true;
                }
                catch (SqlException sqlEx)
                {
                    // Handle SQL-specific errors
                    throw new Exception($"A database error occurred during soft deletion: {sqlEx.Message}", sqlEx);
                }
                catch (Exception ex)
                {
                    // Catch other exceptions
                    throw new Exception("An unexpected error occurred during user soft deletion.", ex);
                }
            }
        }
    }
}