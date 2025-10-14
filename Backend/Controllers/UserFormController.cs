using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.IUserForms;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.UserForms;
using NurseRecordingSystem.DTO.UserServiceDTOs.UserFormsDTOs;

namespace NurseRecordingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // e.g., /api/UserForm
    public class UserFormController : ControllerBase
    {
        private readonly ICreateUserForm _createUserFormService;
        private readonly IDeleteUserForm _deleteUserFormService;
        private readonly IUpdateUserForm _updateUserFormService;

        public UserFormController(ICreateUserForm createUserFormService, IDeleteUserForm deleteUserFormService, IUpdateUserForm updateUserFormService)
        {
            _createUserFormService = createUserFormService;
            _deleteUserFormService = deleteUserFormService;
            _updateUserFormService = updateUserFormService;
        }

        #region POST FORM
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
                UserFormResponseDTO response = await _createUserFormService.CreateUserFormAsync(request, userId, creator);

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
        #endregion

        #region PUT/UPDATE FORM
        /// <summary>
        /// Handles the request to update an existing PatientForm record.
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserForm(
            [FromBody] UpdateUserFormRequestDTO userUpdateFormRequest,
            [FromHeader(Name = "X-UpdatedBy")] string UpdatedByName)
        {
            if (userUpdateFormRequest == null)
            {
                return BadRequest("UserFormRequest cannot be null.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // 3. Call the injected service method
                UserFormResponseDTO response = await _updateUserFormService.UpdateUserFormAsync(userUpdateFormRequest, UpdatedByName);
                if (response.IsSuccess)
                {
                    // HTTP 200 OK with the response DTO
                    return Ok(response);
                }
                else
                {
                    // If the service logic failed to update (e.g., database update failed)
                    return StatusCode(500, "Update failed. Please try again.");
                }
            }
            catch (ArgumentNullException ex)
            {
                // Handle the specific null check from the service layer
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception (recommended)
                // Return a 404 if the specific exception indicates the resource wasn't found
                if (ex.Message.Contains("not found or is already deleted"))
                {
                    return NotFound(ex.Message);
                }
                // Return a 500 Internal Server Error for unhandled exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        #endregion

        #region DELETE FORM
        /// <summary>
        /// Handles the request to softly delete a PatientForm record.
        /// </summary>
        /// <param name="formId">The ID of the form to be deleted.</param>
        [HttpDelete("delete/{formId}")] // Defines the HTTP method and route pattern (e.g., DELETE /api/userform/delete/123)
        
        public async Task<IActionResult> DeleteUserForm(
            int formId, 
            [FromHeader(Name = "X-DeletedBy")] string deletedByName)
        {
            
            // --- Authentication/Authorization Logic (Placeholder) ---
            // Get the ID of the user performing the deletion from the request context.
            // This assumes the user's ID is stored in the JWT token/Claims.
            string deletedBy = deletedByName ?? "Unknown";

            if (string.IsNullOrEmpty(deletedBy) || deletedBy == "Unknown")
            {
                // If the user's ID cannot be determined, return unauthorized.
                return Unauthorized("Cannot determine the identity of the user performing the deletion.");
            }
            // --------------------------------------------------------

            if (formId <= 0)
            {
                return BadRequest("Invalid Form ID provided.");
            }

            try
            {
                // 3. Call the injected service method
                bool success = await _deleteUserFormService.DeleteUserFormAsync(formId, deletedBy);

                if (success)
                {
                    // HTTP 200 OK with a success message
                    return Ok($"Patient Form ID {formId} was successfully deleted.");
                }
                else
                {
                    // If the service logic failed to delete (e.g., database update failed)
                    return StatusCode(500, "Deletion failed. Please try again.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (recommended)

                // Return a 404 if the specific exception indicates the resource wasn't found
                if (ex.Message.Contains("not found or is already deleted"))
                {
                    return NotFound($"Patient Form ID {formId} not found or is already deleted.");
                }

                // Return a 500 Internal Server Error for unhandled exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        #endregion

    }
}