using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.UserForms;
using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UserFormsDTOs;

namespace NurseRecordingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // e.g., /api/UserForm
    public class UserFormController : ControllerBase
    {
        private readonly ICreateUserForm _userFormService;

        public UserFormController(ICreateUserForm userFormService)
        {
            _userFormService = userFormService;
        }

        /// <summary>
        /// Creates a new user form.
        /// </summary>
        /// <param name="request">The data for the new form.</param>
        /// <param name="userId">The ID of the patient/user the form relates to (passed via header or route, depending on your setup).</param>
        /// <param name="creator">The ID of the nurse/creator (passed via header or claims).</param>
        /// <returns>A 201 Created or 400 Bad Request.</returns>
        [HttpPost("create_form")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateForm(
            [FromBody] UserFormRequestDTO request,
            [FromHeader(Name = "X-User-ID")] string userId,
            [FromHeader(Name = "X-Creator-ID")] string creator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // The controller calls the blueprint's method
                UserFormResponseDTO response = await _userFormService.CreateUserForm(request, userId, creator);

                // Assuming successful creation, return 201 Created.
                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (ArgumentNullException ex)
            {
                // Handle the specific null check from the service layer
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Catch generic exceptions (like the SQL error caught in your service)
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred while creating the user form.", details = ex.Message });
            }
        }
    }
}