using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Class.Services.HelperServices;
using NurseRecordingSystem.Contracts.RepositoryContracts.User;
using NurseRecordingSystem.Contracts.ServiceContracts.Auth;
using NurseRecordingSystem.Model.DTO.HelperDTOs;

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
        public async Task<LoginResponseDTO> AuthenticateAsync(LoginRequestDTO request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "LoginRequest cannot be Null");
            }

            using (var connection = new SqlConnection(_connectionString))
            using (var cmdLoginUser = new SqlCommand("dbo.LoginUserAuth", connection))
            {
                cmdLoginUser.CommandType = System.Data.CommandType.StoredProcedure;
                cmdLoginUser.Parameters.AddWithValue("@email", request.Email);
                try
                {

                    await connection.OpenAsync();
                    using (var reader = cmdLoginUser.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            if (PasswordHelper.VerifyPasswordHash(request.Password, (byte[])reader["passwordHash"], (byte[])reader["passwordSalt"]) == true && request.Email == (reader["email"].ToString())) // TODO: use hashing here
                            {
                                return new LoginResponseDTO
                                {
                                    AuthId = int.Parse(reader["authId"].ToString()!),
                                    UserName = reader["userName"].ToString()!,
                                    Email = reader["email"].ToString()!,
                                    Role = int.Parse(reader["role"].ToString()!),
                                    IsAuthenticated = true
                                };
                            }
                        }
                    }

                }
                catch (SqlException ex)
                {
                    throw new Exception("Database ERROR occured during login", ex);
                }

                throw new UnauthorizedAccessException("Invalid Email or Password.");
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
