using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Class.Services.HelperServices;
using NurseRecordingSystem.Contracts.ServiceContracts.User;
using NurseRecordingSystem.Model.DTO.AuthDTOs;
using NurseRecordingSystem.Model.DTO.UserDTOs;

namespace NurseRecordingSystem.Class.Services.UserServices
{
    public class CreateUsersServices : ICreateUsersService
    {
        private readonly string? _connectionString;

        public CreateUsersServices(IConfiguration configuration)
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
            //Integer(1) = User Access
            //CHORE: Updated Insert SqlCommand to accept updatedOn, updatedBy, isActive(bit) :,(
            //Note: Convert userName to isUnique
            //CHORE: Create A DisplayName in Database and DTO for DisplayNames
            //CHORE: Test Response time for this function after convertion of sqlcommands to stored procedures
            var role = 1;
            byte[] passwordSalt, PasswordHash;
            PasswordHelper.CreatePasswordHash(authRequest.Password, out PasswordHash, out passwordSalt);
            int newAuthId;

            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand("dbo.CreateUserAndAuth", connection))
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

        //User Login 
        //CHORE: Updated Insert SqlCommand to accept updatedOn, updatedBy, isActive(bit) :,(
        //Uneeded Function -> Transfered this to CreateUserAuthenticateAsync
        public async Task CreateUserAsync(CreateUserRequestDTO user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }
            var userAuthId = user.AuthId.ToString();
            await using(var connection = new SqlConnection(_connectionString))
            await using(var cmdCreateUser =
                new SqlCommand("INSERT INTO [Users] (authId, firstName, middleName, lastName, contactNumber, address, createdBy, updatedOn, updatedBy, isActive) " +
                "VALUES (@authId, @firstName, @middleName, @lastName, @contactNumber, @address, @createdBy, @updatedOn, @updatedBy, @isActive)", connection))
            {
                cmdCreateUser.Parameters.AddWithValue("@firstName", user.FirstName);
                cmdCreateUser.Parameters.AddWithValue("@middleName", user.MiddleName);
                cmdCreateUser.Parameters.AddWithValue("@lastName", user.LastName);
                cmdCreateUser.Parameters.AddWithValue("@contactNumber", user.ContactNumber);
                cmdCreateUser.Parameters.AddWithValue("@address", user.Address);
                cmdCreateUser.Parameters.AddWithValue("@authId", user.AuthId);
                cmdCreateUser.Parameters.AddWithValue("@createdBy", userAuthId);
                cmdCreateUser.Parameters.AddWithValue("@updatedOn", DateTime.UtcNow);
                cmdCreateUser.Parameters.AddWithValue("@updatedBy", userAuthId);
                cmdCreateUser.Parameters.AddWithValue("@isActive", 1);
                try
                {
                    await connection.OpenAsync();
                    await cmdCreateUser.ExecuteNonQueryAsync();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Database ERROR occured during creating during USER", ex);
                }
            }
        }
    }
}
