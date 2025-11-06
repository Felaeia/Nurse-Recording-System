using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient; // Use this (or System.Data.SqlClient)
using System.Data;

namespace NurseRecordingSystem.Authorization
{
    public class SessionTokenAuthorizationHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _connectionString;

        // Inject IHttpContextAccessor to read cookies
        // Inject IConfiguration to read the connection string
        public SessionTokenAuthorizationHandler(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;

            // Get your connection string from appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RoleRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                context.Fail(); // No HTTP context
                return;
            }

            // Step 1: Read the token string from the cookie
            if (!httpContext.Request.Cookies.TryGetValue("SessionToken", out var tokenString))
            {
                context.Fail(); // No session token cookie
                return;
            }

            byte[] tokenBytes;
            try
            {
                // Step 2: Convert the Base64 string back to bytes
                tokenBytes = Convert.FromBase64String(tokenString);
            }
            catch
            {
                context.Fail(); // Invalid token format
                return;
            }

            // Step 3: Call the Stored Procedure
            string userRole = null;
            await using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    await using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "sp_ValidateSessionTokenAndGetRole";
                        command.CommandType = CommandType.StoredProcedure;

                        // Add the @Token parameter
                        command.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "@Token",
                            Value = tokenBytes,
                            SqlDbType = SqlDbType.VarBinary, // Be explicit
                            Size = 64
                        });

                        // ExecuteScalarAsync is perfect for getting a single value
                        var result = await command.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                        {
                            userRole = (string)result;
                        }
                    }
                }
                catch (Exception)
                {
                    // Log the exception here
                    context.Fail(); // Failed to query database
                    return;
                }
            } // Connection is automatically closed here

            // Step 4: Validate the role
            if (userRole == null)
            {
                context.Fail(); // SP returned no role (token invalid/expired)
                return;
            }

            // Step 5:
            // Check if the role from the SP is in the requirement's list
            if (requirement.AllowedRoles.Contains(userRole))
            {
                // Success! The user's role is one of the allowed roles.
                context.Succeed(requirement);
            }
            else
            {
                context.Fail(); // User's role is not in the allowed list
            }
        }
    }
}