using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.Users;
using NurseRecordingSystem.DTO.UserServiceDTOs.UsersDTOs;

namespace NurseRecordingSystem.Class.Services.UserServices.Users
{
    public class ViewUserProfile : IViewUserProfile
    {
        private readonly string? _connectionString;

        public ViewUserProfile(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<ViewUserProfileDTO> GetUserProfileAsync(int userId)
        {
            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand("dbo.usp_GetUserProfileById", connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", userId);

                try
                {
                    await connection.OpenAsync();
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Map the data returned by the stored procedure to a DTO
                            return new ViewUserProfileDTO
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("userId")),
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                FirstName = reader.GetString(reader.GetOrdinal("firstName")),
                                MiddleName = reader.IsDBNull(reader.GetOrdinal("middleName")) ? null : reader.GetString(reader.GetOrdinal("middleName")),
                                LastName = reader.GetString(reader.GetOrdinal("lastName")),
                                ContactNumber = reader.GetString(reader.GetOrdinal("contactNumber")),
                                Address = reader.IsDBNull(reader.GetOrdinal("address")) ? null : reader.GetString(reader.GetOrdinal("address")),
                                Role = reader.GetInt32(reader.GetOrdinal("role"))
                            };
                        }
                        else
                        {
                            throw new Exception($"User with ID {userId} not found or is inactive.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Catch specific exceptions if possible, otherwise throw generic data access error
                    throw new Exception("An error occurred while retrieving user profile.", ex);
                }
            }
        }
    }
}