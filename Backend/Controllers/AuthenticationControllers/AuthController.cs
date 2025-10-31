using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ControllerContracts;
using NurseRecordingSystem.Contracts.ServiceContracts.Auth;
using NurseRecordingSystem.DTO.AuthServiceDTOs;

namespace NurseRecordingSystem.Controllers.AuthenticationControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase, IAuthController
    {
        private readonly IUserAuthenticationService _userAuthenticationService;
        // 1. Add the Session Token Service
        private readonly ISessionTokenService _sessionTokenService;

        // 2. Inject the service in the constructor
        public AuthController(
            IUserAuthenticationService userAuthenticationService,
            ISessionTokenService sessionTokenService)
        {
            _userAuthenticationService = userAuthenticationService
                ?? throw new ArgumentNullException(nameof(userAuthenticationService), "UserAuthentication cannot be null");

            // 3. Assign the injected service
            _sessionTokenService = sessionTokenService
                ?? throw new ArgumentNullException(nameof(sessionTokenService), "SessionTokenService cannot be null");
        }

        #region User Login (My eyes are huritng TT)
        /// <summary>
        /// Authenticates a user and provides a session token.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequestDTO loginUser)
        {
            try
            {
                // Step A: Authenticate the user's email and password
                var authResponse = await _userAuthenticationService.AuthenticateAsync(new LoginRequestDTO
                {
                    Email = loginUser.Email,
                    Password = loginUser.Password
                });

                if (authResponse == null)
                {
                    return Unauthorized("Invalid credentials.");
                }

                // Step B: Validate if an active session already exists
                bool isTokenValid = await _sessionTokenService.ValidateTokenAsync(authResponse.AuthId);

                SessionTokenDTO? tokenResponse;

                if (isTokenValid)
                {
                    // A valid token exists, so refresh it
                    tokenResponse = await _sessionTokenService.RefreshSessionTokenAsync(authResponse.AuthId);
                }
                else
                {
                    // No valid token exists, so create a new one
                    tokenResponse = await _sessionTokenService.CreateSessionAsync(authResponse.AuthId);
                }

                // Step C: Check if token creation/refresh was successful
                if (tokenResponse == null)
                {
                    // This might happen if Create fails or Refresh fails (e.g., token expired between check and refresh)
                    return StatusCode(500, "Login successful but failed to create or refresh a session token.");
                }

                // Step D: Return the single, successful response
                return Ok(new
                {
                    User = authResponse,
                    Token = tokenResponse, // Send the new/refreshed token to the client
                    Message = "Login Successful"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error in Login: {ex.Message}");
            }
        }
        #endregion
    }
}