using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ControllerContracts;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.Users;
using NurseRecordingSystem.DTO.UserServiceDTOs.UsersDTOs;
using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UsersDTOs;

namespace NurseRecordingSystem.Controllers.UserControllers
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
        public async Task<IActionResult> CreateAuthentication([FromBody] CreateAuthenticationRequestDTO request)
        {
            try
            {
                var authRequest = new CreateAuthenticationRequestDTO
                {
                    UserName = request.UserName,
                    Password = request.Password,
                    Email = request.Email
                };

                var userRequest = new CreateUserRequestDTO
                {
                    FirstName = request.FirstName,
                    MiddleName = request.MiddleName,
                    LastName = request.LastName,
                    Address = request.Address,
                    ContactNumber = request.ContactNumber
                };
                var authId = await _createUsersService.CreateUserAuthenticateAsync(authRequest, userRequest);
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
