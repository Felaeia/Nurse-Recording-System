using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Class.Services.HelperServices.HelperAuthentication;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.NurseCreation;
using NurseRecordingSystem.Model.DTO.NurseServicesDTOs.NurseCreation;

namespace NurseRecordingSystem.Class.Services.NurseServices.NurseCreation
{
    public class CreateNurse : ICreateNurse
    {
        private readonly string? _connectionString;

        public CreateNurse(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<int> CreateNurseAsync(CreateNurseRequestDTO request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Authentication cannot be null");
            }
            byte[] passwordSalt, PasswordHash;
            PasswordHelper.CreatePasswordHash(request.Password, out PasswordHash, out passwordSalt);

            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand("dbo.asp_CreateNurse", connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Parameters for [Auth] table
                cmd.Parameters.AddWithValue("@userName", request.UserName);
                cmd.Parameters.AddWithValue("@passwordHash", PasswordHash);
                cmd.Parameters.AddWithValue("@passwordSalt", passwordSalt);
                cmd.Parameters.AddWithValue("@email", request.Email);
                cmd.Parameters.AddWithValue("@createdBy", "Admin");
                cmd.Parameters.AddWithValue("@updatedOn", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@updatedBy", "Admin");
                cmd.Parameters.AddWithValue("@isActive", 1);

                // Parameters for [Users] table
                cmd.Parameters.AddWithValue("@firstName", request.FirstName);
                cmd.Parameters.AddWithValue("@middleName", request.MiddleName);
                cmd.Parameters.AddWithValue("@lastName", request.LastName);
                cmd.Parameters.AddWithValue("@contactNumber", request.ContactNumber);

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
                    throw new Exception("Database ERROR occured during creating AUTH & Nurse", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while creating authentication and user.", ex);
                }
            }
        }
    }
}