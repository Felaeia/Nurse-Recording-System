using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseUserForms;
using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UserFormsDTOs;

namespace NurseRecordingSystem.API.Controllers
{
    // Assuming this is used by nursing staff to see a dashboard list
    [Route("api/[controller]")]
    [ApiController]
    public class NurseUserFormController : ControllerBase
    {
        private readonly IViewUserFormList _viewUserFormListService;

        public NurseUserFormController(IViewUserFormList viewUserFormListService)
        {
            _viewUserFormListService = viewUserFormListService;
        }

        // GET: api/NurseUserForm/list
        // GET: api/NurseUserForm/list?userId=101&status=Pending
        /// <summary>
        /// Retrieves a summary list of patient forms with optional filters.
        /// </summary>
        [HttpGet("user/form_list")]
        [Authorize(Policy = "MustBeNurse")]
        [ProducesResponseType(typeof(List<UserFormListItemDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserFormList(
            [FromQuery] int? userId,
            [FromQuery] string? status)
        {
            try
            {
                var forms = await _viewUserFormListService.GetUserFormListAsync(userId, status);

                // Return 200 OK with an empty list if no results are found
                return Ok(forms);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while retrieving the list of patient forms."
                );
            }
        }
    }
}