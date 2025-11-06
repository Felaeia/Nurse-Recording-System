using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ServiceContracts.IAdminServices.IAdminUser;

namespace NurseRecordingSystem.Controllers.AdminControllers
{
    // Sets the base route for the controller to api/adminusers
    [Route("api/[controller]")]
    [ApiController]
    // Recommended: Add authorization here to restrict access to administrators
    // [Authorize(Roles = "Admin, SuperAdmin")] 
    public class AdminUsersController : ControllerBase
    {
        private readonly IDeleteUser _deleteUserService;

        // Dependency Injection: Inject the IDeleteUser service
        public AdminUsersController(IDeleteUser deleteUserService)
        {
            _deleteUserService = deleteUserService;
        }

        // DELETE: api/AdminUsers/Delete/{userId}
        [HttpDelete("delete/user/{userId}")]
        [Authorize(Policy = "MustBeNurse")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)] // Success: Action taken, no content to return
        //[ProducesResponseType(StatusCodes.Status400BadRequest)] // Invalid input or missing header
        //[ProducesResponseType(StatusCodes.Status404NotFound)] // User not found (handled by custom service exception)
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)] // Generic internal error
        public async Task<IActionResult> SoftDeleteUser(
            int userId,
            [FromHeader(Name = "X-Deleted-By")] string deletedBy // Assuming identity comes from a custom header
        )
        {
            // 1. Input Validation and Authentication Check (Basic)
            if (string.IsNullOrWhiteSpace(deletedBy))
            {
                // In a real application, you would typically use the authenticated user's identity 
                // from HttpContext.User.Identity.Name instead of a header.
                return BadRequest("The 'X-Deleted-By' header is required for auditing.");
            }

            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            try
            {
                // 2. Business Logic: Call the soft delete service
                bool isDeleted = await _deleteUserService.SoftDeleteUserAsync(userId, deletedBy);

                if (isDeleted)
                {
                    // Success: Return 204 No Content as the resource is conceptually gone/modified, and no body is needed.
                    return NoContent();
                }
                else
                {
                    // This path is unlikely given the service implementation, but serves as a fallback
                    return StatusCode(StatusCodes.Status500InternalServerError, "Deletion failed for unknown reason.");
                }
            }
            catch (Exception ex)
            {
                // 3. Error Handling

                // Check for custom exception messages from the service (e.g., user not found)
                if (ex.Message.Contains("User not found"))
                {
                    return NotFound($"User with ID {userId} not found or is already deleted.");
                }
                // Return 500 Internal Server Error for all other failures (e.g., database connection issues)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while attempting to soft delete the user."
                );
            }
        }
    }
}
