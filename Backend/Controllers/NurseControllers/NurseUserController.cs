using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseUsers;
using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UsersDTOs;

namespace NurseRecordingSystem.Controllers.NurseControllers
{
    // Sets the base route for the controller to api/nurseuser
    [Route("api/[controller]")]
    [ApiController]
    // Note: You would typically add authorization attributes here, e.g., [Authorize(Roles = "Admin, SuperAdmin")]
    public class NurseUserController : ControllerBase
    {
        private readonly IViewAllUsers _viewAllUsersService;

        // Dependency Injection: The IViewAllUsers service is injected into the controller
        public NurseUserController(IViewAllUsers viewAllUsersService)
        {
            _viewAllUsersService = viewAllUsersService;
        }

        // GET: api/NurseUser/all (Retrieves all users)
        // GET: api/NurseUser/all?isActive=true (Retrieves only active users)
        [HttpGet("view/all_users")]
        [ProducesResponseType(typeof(List<ViewAllUsersDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // Accepts an optional query parameter 'isActive' to filter the results
        public async Task<IActionResult> GetAllUsers([FromQuery] bool? isActive)
        {
            try
            {
                // Business Logic: Call the service to get the user data
                var users = await _viewAllUsersService.GetAllUsersAsync(isActive);

                // Check if the list is empty (can return 200 OK with an empty list, or 404 Not Found)
                if (users == null || users.Count == 0)
                {
                    // Returning 200 OK with an empty list is common for "Get All" endpoints
                    return Ok(new List<ViewAllUsersDTO>());
                }

                // Success: Return the list of users with a 200 OK status
                return Ok(users);
            }
            catch (Exception)
            {
                // Error Handling: Log the error and return a 500 Internal Server Error
                // In a real application, logging would happen here.
                //Console.LogError($"Error retrieving all users: {ex.Message}", ex);

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while retrieving the list of users."
                );
            }
        }
    }
}