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
            var role = 1;
            byte[] passwordSalt, PasswordHash;
            PasswordHelper.CreatePasswordHash(authRequest.Password, out PasswordHash, out passwordSalt);
            int newAuthId;

            await using (var connection = new SqlConnection(_connectionString))
            await using(var cmdUserAuth =
                new SqlCommand("INSERT INTO [Auth] (userName, passwordHash, passwordSalt, email, role, createdBy, updatedOn, updatedBy, isActive) " +
                "VALUES (@userName, @passwordHash, @passwordSalt, @email, @role, @createdBy, @updatedOn, @updatedBy, @isActive);" +
                "SELECT CAST(SCOPE_IDENTITY() as int);", connection))
            using (var cmdCreateSessionToken = new SqlCommand("INSERT INTO [SessionTokens] (token, userId, expiresOn, isActive) " +
                "VALUES (@CreatedToken, @UserId, @ExpiresOn, @IsActive) " +
                "SELECT CAST (SCOPE_IDENTITY() as int)", connection)) 
            {
                //Insert into Auth Table
                cmdUserAuth.Parameters.AddWithValue("@userName", authRequest.UserName);
                cmdUserAuth.Parameters.AddWithValue("@passwordHash", PasswordHash);
                cmdUserAuth.Parameters.AddWithValue("@passwordSalt", passwordSalt);
                cmdUserAuth.Parameters.AddWithValue("@email", authRequest.Email);
                cmdUserAuth.Parameters.AddWithValue("@role", role);
                cmdUserAuth.Parameters.AddWithValue("@createdBy", authRequest.UserName);
                cmdUserAuth.Parameters.AddWithValue("@updatedOn", DateTime.UtcNow); 
                cmdUserAuth.Parameters.AddWithValue("@updatedBy", authRequest.UserName);
                cmdUserAuth.Parameters.AddWithValue("@isActive", 1);
                try
                {
                    await connection.OpenAsync();
                    var result = await cmdUserAuth.ExecuteScalarAsync();
                    if (result == null || result == DBNull.Value)
                    {
                        throw new Exception("Failed to retrieve the new authentication ID.");
                    }
                    newAuthId = (int)result;
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException("Database ERROR occured during creating AUTHENTICATION", ex);
                }
            }

            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmdCreateUser =
                new SqlCommand("INSERT INTO [Users] (authId, firstName, middleName, lastName, contactNumber, address, createdBy, updatedOn, updatedBy, isActive) " +
                "VALUES (@authId, @firstName, @middleName, @lastName, @contactNumber, @address, @createdBy, @updatedOn, @updatedBy, @isActive)", connection))
            {
                //Insert into User Table
                cmdCreateUser.Parameters.AddWithValue("@firstName", user.FirstName);
                cmdCreateUser.Parameters.AddWithValue("@middleName", user.MiddleName);
                cmdCreateUser.Parameters.AddWithValue("@lastName", user.LastName);
                cmdCreateUser.Parameters.AddWithValue("@contactNumber", user.ContactNumber);
                cmdCreateUser.Parameters.AddWithValue("@address", user.Address);
                cmdCreateUser.Parameters.AddWithValue("@authId", newAuthId);
                cmdCreateUser.Parameters.AddWithValue("@createdBy", authRequest.UserName);
                cmdCreateUser.Parameters.AddWithValue("@updatedOn", DateTime.UtcNow);
                cmdCreateUser.Parameters.AddWithValue("@updatedBy", authRequest.UserName);
                cmdCreateUser.Parameters.AddWithValue("@isActive", 1);

                try
                {
                    await connection.OpenAsync();
                    await cmdCreateUser.ExecuteNonQueryAsync();
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException("Database ERROR occured during creating User", ex);
                }
            }
            return newAuthId;
        }

        //User Login 
        //CHORE: Updated Insert SqlCommand to accept updatedOn, updatedBy, isActive(bit) :,(
        public async Task CreateUser(CreateUserRequestDTO user)
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
