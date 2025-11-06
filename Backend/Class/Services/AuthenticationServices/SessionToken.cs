using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.Auth;
using NurseRecordingSystem.DTO.AuthServiceDTOs;
using System.Data;

namespace NurseRecordingSystem.Class.Services.Authentication
{
    public class SessionTokenService : ISessionTokenService
    {
        private readonly string? _connectionString;

        // Dependency Injection of IConfiguration
        public SessionTokenService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        /// <summary>
        /// Creates a new session token and deactivates any old ones.
        /// </summary>
        public async Task<SessionTokenDTO?> CreateSessionAsync(int authId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("dbo.usp_CreateSessionToken", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@authId", authId);

                try
                {
                    await connection.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapReaderToSessionTokenDTO(reader);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Log the exception (logging framework recommended)
                    throw new Exception("Database error creating session token.", ex);
                }
            }
            return null; // Should not happen if SP is correct
        }

        /// <summary>
        /// Updates an existing active token with a new value and expiry.
        /// </summary>
        public async Task<SessionTokenDTO?> RefreshSessionTokenAsync(int authId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("dbo.usp_UpdateSessionToken", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@authId", authId);

                try
                {
                    await connection.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapReaderToSessionTokenDTO(reader);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Log the exception
                    throw new Exception("Database error refreshing session token.", ex);
                }
            }
            return null; // No active token was found to refresh
        }

        /// <summary>
        /// Deactivates the user's session token based on the token value.
        /// </summary>
        /// <returns>True if the session was successfully ended, false otherwise.</returns>
        public async Task<bool> EndSessionAsync(byte[] tokenBytes)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("dbo.usp_EndSessionToken", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // 1. Change the parameter to @Token
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Token",
                    Value = tokenBytes,
                    SqlDbType = SqlDbType.VarBinary,
                    Size = 64
                });

                try
                {
                    await connection.OpenAsync();

                    // 2. Execute and get the number of rows affected
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    // 3. Return true if we successfully updated 1 row
                    return rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    // Log the exception
                    throw new Exception("Database error ending session.", ex);
                }
            }
        }

        /// <summary>
        /// Helper method to map SqlDataReader to DTO.
        /// </summary>
        private SessionTokenDTO MapReaderToSessionTokenDTO(SqlDataReader reader)
        {
            return new SessionTokenDTO
            {
                TokenId = (int)reader["tokenId"],
                Token = (byte[])reader["token"],
                AuthId = (int)reader["authId"],
                ExpiresOn = (DateTime)reader["expiresOn"],
                IsActive = (int)reader["isActive"]
            };
        }

        /// <summary>
        /// Validates if an active, non-expired session exists for a user.
        /// </summary>
        public async Task<bool> ValidateTokenAsync(int authId) // <-- Signature changed
        {
            // Remove the old byte[] token check

            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("dbo.usp_ValidateSessionToken", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // 1. Pass in the authId parameter
                cmd.Parameters.AddWithValue("@authId", authId);

                try
                {
                    await connection.OpenAsync();

                    // 2. Use ExecuteScalarAsync to get the single (bool) return value
                    // The stored procedure will return a bit (0 or 1), 
                    // which ExecuteScalarAsync reads as a boolean.
                    var result = await cmd.ExecuteScalarAsync();

                    // 3. Cast and return the result
                    return (bool)result;
                }
                catch (SqlException ex)
                {
                    // Log the exception
                    throw new Exception("Database error validating session.", ex);
                }
                catch (Exception ex)
                {
                    // Handle potential null or casting errors, though the SP should always return a bit
                    throw new Exception("Error processing validation result.", ex);
                }
            }
        }
    }
}