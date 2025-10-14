using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Class.Services.HelperServices.HelperAuthentication;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.Users;
using NurseRecordingSystem.DTO.UserServiceDTOs.UsersDTOs;

namespace NurseRecordingSystem.Class.Services.UserServices.Users
{
    public class CreateUser : ICreateUsers
    {
        private readonly string? _connectionString;

        public CreateUser(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        //Create Auth for User Function (role = user)
        public async Task<int> CreateUserAuthenticateAsync(CreateAuthenticationRequestDTO authRequest, CreateUserRequestDTO user)
        {
            if (authRequest == null)
            {
                throw new ArgumentNullException(nameof(authRequest), "Authentication cannot be null");
            }
            var role = 1;
            byte[] passwordSalt, PasswordHash;
            PasswordHelper.CreatePasswordHash(authRequest.Password, out PasswordHash, out passwordSalt);

            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand("dbo.usp_CreateUserAndAuth", connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Parameters for [Auth] table
                cmd.Parameters.AddWithValue("@userName", authRequest.UserName);
                cmd.Parameters.AddWithValue("@passwordHash", PasswordHash);
                cmd.Parameters.AddWithValue("@passwordSalt", passwordSalt);
                cmd.Parameters.AddWithValue("@email", authRequest.Email);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.Parameters.AddWithValue("@createdBy", "System");
                cmd.Parameters.AddWithValue("@updatedOn", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@updatedBy", "System");
                cmd.Parameters.AddWithValue("@isActive", 1);

                // Parameters for [Users] table
                cmd.Parameters.AddWithValue("@firstName", user.FirstName);
                cmd.Parameters.AddWithValue("@middleName", user.MiddleName);
                cmd.Parameters.AddWithValue("@lastName", user.LastName);
                cmd.Parameters.AddWithValue("@contactNumber", user.ContactNumber);
                cmd.Parameters.AddWithValue("@address", user.Address);

                try
                {
                    await connection.OpenAsync();

                    var result = await cmd.ExecuteScalarAsync();

                    if (result == null || (int)result <= 0)
                    {
                        throw new Exception("Failed to create authentication record.");
                    }

                    return (int)result;
                }
                catch (SqlException ex) when (ex.Message.Contains("Email already existing"))
                {
                    throw new InvalidOperationException("The email address provided is already in use.", ex);
                }
                catch (SqlException ex)
                {
                    throw new Exception("Database ERROR occured during creating AUTH & USER", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while creating authentication and user.", ex);
                }
            }
        }
    }
}
