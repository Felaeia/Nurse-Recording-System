using Microsoft.AspNetCore.Authorization;
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

        #region User LOGIN Endpoint
        /// <summary>
        /// Authenticates a user and provides a session token.
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
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

                    #region COOKIE LOGIC

                    // 1. Convert the token's byte[] to a URL-safe string
                    var tokenString = Convert.ToBase64String(tokenResponse.Token);

                    // 2. Define the cookie options for security
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = tokenResponse.ExpiresOn
                    };

                    // 3. Add the cookie to the HTTP response
                    Response.Cookies.Append("SessionToken", tokenString, cookieOptions);

                    #endregion
                }
                else
                {
                    // No valid token exists, so create a new one
                    tokenResponse = await _sessionTokenService.CreateSessionAsync(authResponse.AuthId);

                    #region COOKIE LOGIC

                    // 1. Convert the token's byte[] to a URL-safe string
                    var tokenString = Convert.ToBase64String(tokenResponse.Token);

                    // 2. Define the cookie options for security
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = tokenResponse.ExpiresOn
                    };

                    // 3. Add the cookie to the HTTP response
                    Response.Cookies.Append("SessionToken", tokenString, cookieOptions);

                    #endregion
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

        #region User LOGOUT Endpoint
        /// <summary>
        /// Logs out a user by invalidating their session token.
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutUser()
        {
            try
            {
                // 1. Retrieve the SessionToken from cookies
                if (!Request.Cookies.TryGetValue("SessionToken", out string? tokenString) || string.IsNullOrEmpty(tokenString))
                {
                    return BadRequest("No session token found in cookies.");
                }
                // 2. Convert the token string back to byte[]
                byte[] tokenBytes;
                try
                {
                    tokenBytes = Convert.FromBase64String(tokenString);
                }
                catch (FormatException)
                {
                    return BadRequest("Invalid session token format.");
                }
                // 3. Invalidate the session token
                bool logoutSuccess = await _sessionTokenService.EndSessionAsync(tokenBytes);
                // 4. Remove the cookie from the client
                Response.Cookies.Delete("SessionToken");
                return Ok("Logout successful.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error in Logout: {ex.Message}");
            }
        }
        #endregion
    }
}