using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ControllerContracts;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.Users;
using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UsersDTOs;

namespace PresentationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateUserController : ControllerBase , IUserController
    {
        private readonly ICreateUsers _createUsersService;

        public CreateUserController(ICreateUsers createUsersService)
        {
            _createUsersService = createUsersService
                ?? throw new ArgumentNullException(nameof(createUsersService), "UserAuthentication cannot be null");
        }

        #region Post User
        /// <summary>
        /// Create authentication (login credentials) for a new user.
        /// </summary>
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateAuthentication([FromBody] CreateAuthenticationRequestDTO aRequest, [FromBody] CreateUserRequestDTO uRequest)
        {
            try
            {
                var authId = await _createUsersService.CreateUserAuthenticateAsync(aRequest, uRequest);
                //await _createUsersService.CreateUserAsync(userRequest);
                return Ok(new { AuthId = authId, Message = "Authentication created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
        #endregion
    }
}
