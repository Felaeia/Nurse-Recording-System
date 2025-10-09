using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.User;
using NurseRecordingSystem.Model.DTO.UserDTOs; // Assuming an UpdateUserRequestDTO exists

namespace NurseRecordingSystem.Class.Services.UserServices.Users
{
    public class UpdateUser : IUpdateUserService
    {
        private readonly string? _connectionString;

        public UpdateUser(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<bool> UpdateUserProfileAsync(int userId, UpdateUserRequestDTO userRequest, string updatedBy)
        {
            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand("dbo.usp_UpdateUserProfile", connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Input Parameters
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@email", userRequest.Email);
                cmd.Parameters.AddWithValue("@firstName", userRequest.FirstName);
                cmd.Parameters.AddWithValue("@middleName", (object)userRequest.MiddleName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@lastName", userRequest.LastName);
                cmd.Parameters.AddWithValue("@contactNumber", userRequest.ContactNumber);
                cmd.Parameters.AddWithValue("@address", (object)userRequest.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@updatedBy", updatedBy);

                // Output/Return Parameter from SP
                var returnValue = cmd.Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int);
                returnValue.Direction = System.Data.ParameterDirection.ReturnValue;

                try
                {
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    // Check the return code from the stored procedure
                    int result = (int)returnValue.Value;
                    return result == 0; // 0 for success
                }
                catch (SqlException ex)
                {
                    throw new Exception("Database error occurred during user profile update.", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("An unexpected error occurred during user profile update.", ex);
                }
            }
        }
    }
}