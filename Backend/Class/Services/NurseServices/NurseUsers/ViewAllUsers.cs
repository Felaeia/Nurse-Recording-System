using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseUsers;
using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UsersDTOs; // Assuming ViewUserDTO

namespace NurseRecordingSystem.Class.Services.UserServices.Users
{
    public class ViewAllUsers : IViewAllUsers
    {
        private readonly string? _connectionString;

        public ViewAllUsers(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        // Handles the business logic of receiving all user data from nsp_ViewAllUsers.sp
        public async Task<List<ViewUserDTO>> GetAllUsersAsync(bool? isActive = null) // Optional parameter
        {
            var users = new List<ViewUserDTO>();

            // SQL command and connection setup
            const string storedProc = "dbo.nsp_ViewAllUsers";
            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand(storedProc, connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Add the isActive parameter if a value is provided
                if (isActive.HasValue)
                {
                    cmd.Parameters.AddWithValue("@IsActive", isActive.Value);
                }
                else
                {
                    // If isActive is null, send DBNull.Value to retrieve all users (as defined in the SP)
                    cmd.Parameters.AddWithValue("@IsActive", DBNull.Value);
                }

                try
                {
                    await connection.OpenAsync();
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // Map the data returned by the stored procedure to a DTO
                            users.Add(new ViewUserDTO
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("userId")),
                                UserName = reader.GetString(reader.GetOrdinal("userName")),
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                Role = reader.GetString(reader.GetOrdinal("role")), // Assuming role is nvarchar
                                FirstName = reader.GetString(reader.GetOrdinal("firstName")),
                                MiddleName = reader.IsDBNull(reader.GetOrdinal("middleName")) ? null : reader.GetString(reader.GetOrdinal("middleName")),
                                LastName = reader.GetString(reader.GetOrdinal("lastName")),
                                ContactNumber = reader.GetString(reader.GetOrdinal("contactNumber")),
                                Address = reader.IsDBNull(reader.GetOrdinal("address")) ? null : reader.GetString(reader.GetOrdinal("address")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("isActive")),
                                CreatedOn = reader.GetDateTime(reader.GetOrdinal("createdOn")),
                                CreatedBy = reader.GetString(reader.GetOrdinal("createdBy")),
                                UpdatedOn = reader.GetDateTime(reader.GetOrdinal("updatedOn")),
                                UpdatedBy = reader.GetString(reader.GetOrdinal("updatedBy")),
                            });
                        }
                    }
                    return users;
                }
                catch (SqlException sqlEx)
                {
                    // Handle SQL-specific errors (e.g., connection issue, stored procedure error)
                    throw new Exception($"A database error occurred while retrieving users: {sqlEx.Message}", sqlEx);
                }
                catch (Exception ex)
                {
                    // Catch other exceptions (e.g., mapping error)
                    throw new Exception("An unexpected error occurred while processing user data.", ex);
                }
            }
        }
    }
}