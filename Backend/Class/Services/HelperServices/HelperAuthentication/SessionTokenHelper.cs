using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

namespace NurseRecordingSystem.Class.Services.HelperServices.HelperAuthentication
{
    public class sessionTokenHelper
    {

        private readonly string? _connectionString;

        public sessionTokenHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnectionString")
                ?? throw new InvalidOperationException("Connection string 'Default Connecection' not Found");

        }

        // Create Session Token ( During First Login )
        public void CreateSessionToken(int UserId)
        {
            byte[] token = new byte[64];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("INSERT INTO SessionTokens (Token, UserId, ExpiryDate, IsActive) VALUES (@token, @userId, @expiryDate, @isActive)", con))
            {
                randomNumberGenerator.GetBytes(token);
                cmd.Parameters.AddWithValue("@token", token);
                cmd.Parameters.AddWithValue("@userId", UserId);
                var expiryDate = DateTime.UtcNow.AddHours(720);
                cmd.Parameters.AddWithValue("@expiryDate", expiryDate); // Token valid for 720 hours
                cmd.Parameters.AddWithValue("@isActive", true);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Validate Session Token ( After initial login )
        // Chore: Need Testing
        public bool ValidateToken(byte[] token)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT COUNT(1) FROM SessionTokens WHERE Token = @token AND ExpiryDate > GETUTCDATE()", con))
            {
                cmd.Parameters.AddWithValue("@token", token);
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        // Renew Session Token ( After initial login & When Session expires )
        public void RenewSessionToken(byte[] token)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("UPDATE SessionTokens SET ExpiryDate = @newExpiryDate WHERE Token = @token", con))
            {
                var newExpiryDate = DateTime.UtcNow.AddHours(720); // Extend by 1 hour
                cmd.Parameters.AddWithValue("@newExpiryDate", newExpiryDate);
                cmd.Parameters.AddWithValue("@token", token);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Revoke Session Token
        public void RevokeSessionToken(byte[] token)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("UPDATE SessionTokens SET IsActive = 0 WHERE Token = @token", con))
            {
                cmd.Parameters.AddWithValue("@token", token);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Delete Expired Tokens
        public void DeleteExpiredTokens()
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("DELETE FROM SessionTokens WHERE ExpiryDate <= GETUTCDATE()", con))
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Get User ID from Token
        public int? GetUserIdFromToken(byte[] token)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT UserId FROM SessionTokens WHERE Token = @token AND ExpiryDate > GETUTCDATE()", con))
            {
                cmd.Parameters.AddWithValue("@token", token);
                con.Open();
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return (int)result;
                }
                return null;
            }
        }

    }
}
