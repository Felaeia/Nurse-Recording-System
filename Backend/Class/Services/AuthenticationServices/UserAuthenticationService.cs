using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Class.Services.HelperServices.HelperAuthentication;
using NurseRecordingSystem.Contracts.RepositoryContracts.User;
using NurseRecordingSystem.Contracts.ServiceContracts.Auth;
using NurseRecordingSystem.DTO.AuthServiceDTOs;

namespace NurseRecordingSystem.Class.Services.Authentication
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly string? _connectionString;
        private readonly IUserRepository _userRepository;


        //Dependency Injection of IConfiguration and IUserRepository
        public UserAuthenticationService(IConfiguration configuration, IUserRepository userRepository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _userRepository = userRepository
                ?? throw new ArgumentNullException(nameof(userRepository),"UserAuth Service cannot be null");
        }

        //User Method: Login
        #region Login

        public async Task<LoginResponseDTO?> AuthenticateAsync(LoginRequestDTO request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "LoginRequest cannot be Null");
            }

            using (var connection = new SqlConnection(_connectionString))
            using (var cmdLoginUser = new SqlCommand("dbo.ausp_LoginUserAuth", connection))
            {
                cmdLoginUser.CommandType = System.Data.CommandType.StoredProcedure;
                cmdLoginUser.Parameters.AddWithValue("@email", request.Email);

                try
                {
                    await connection.OpenAsync();
                    using (var reader = await cmdLoginUser.ExecuteReaderAsync()) // Use ExecuteReaderAsync
                    {
                        if (await reader.ReadAsync()) // Use ReadAsync
                        {
                            // First, verify the password
                            if (PasswordHelper.VerifyPasswordHash(request.Password,
                                (byte[])reader["passwordHash"], (byte[])reader["passwordSalt"]))
                            {
                                string userRole = reader["role"].ToString()!;

                                // Create the base response
                                var response = new LoginResponseDTO
                                {
                                    AuthId = (int)reader["authId"],
                                    UserName = reader["userName"].ToString()!,
                                    Email = reader["email"].ToString()!,
                                    Role = userRole,
                                    IsAuthenticated = true
                                };

                                if (userRole == "User")
                                {
                                    response.UserDetails = new UserDetailsDTO
                                    {
                                        UserId = (int)reader["userId"],
                                        FirstName = reader["User_firstName"].ToString()!,
                                        // Check for DBNull before reading nullable fields
                                        MiddleName = reader["User_middleName"] == DBNull.Value ? null : reader["User_middleName"].ToString(),
                                        LastName = reader["User_lastName"].ToString()!,
                                        ContactNumber = reader["User_contactNumber"].ToString()!,
                                        Address = reader["User_address"] == DBNull.Value ? null : reader["User_address"].ToString()
                                    };
                                }
                                else if (userRole == "Nurse")
                                {
                                    response.NurseDetails = new NurseDetailsDTO
                                    {
                                        NurseId = (int)reader["nurseId"],
                                        FirstName = reader["Nurse_firstName"].ToString()!,
                                        MiddleName = reader["Nurse_middleName"] == DBNull.Value ? null : reader["Nurse_middleName"].ToString(),
                                        LastName = reader["Nurse_lastName"].ToString()!,
                                        ContactNumber = reader["Nurse_contactNumber"] == DBNull.Value ? null : reader["Nurse_contactNumber"].ToString()
                                    };
                                }

                                return response;
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Database ERROR occurred during login", ex);
                }

                return null; // Invalid credentials (password wrong or user not found)
            }
        }
        #endregion


        //User Function: To Determine the role of the user
        public async Task<int> DetermineRoleAync(LoginResponseDTO response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response), "LoginResponse cannot be Null");
            }
            var user = await _userRepository.GetUserByUsernameAsync(response.UserName);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }
            return user.Role;
        }

        //User Method: Logout (fishballs need session tokens)
        public async Task LogoutAsync()
        {
            // Implement logout logic if needed (e.g., invalidate tokens, clear session data)
            await Task.CompletedTask;
        }
    }
}
