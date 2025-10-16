using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.HelperContracts.IHelperUserForm;

namespace NurseRecordingSystem.API.Controllers
{
    // Assuming this is used by helper staff to view detailed forms
    [Route("api/[controller]")]
    [ApiController]
    public class HelperUserFormController : ControllerBase
    {
        private readonly IViewUserForm _viewUserFormService;

        public HelperUserFormController(IViewUserForm viewUserFormService)
        {
            _viewUserFormService = viewUserFormService;
        }

        // GET: api/HelperUserForm/5
        /// <summary>
        /// Retrieves the full details of a single patient form by ID.
        /// </summary>
        [HttpGet("{formId}")]
        [ProducesResponseType(typeof(ViewUserFormResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserForm(int formId)
        {
            try
            {
                var form = await _viewUserFormService.GetUserFormAsync(formId);
                return Ok(form);
            }
            catch (KeyNotFoundException)
            {
                // Catches the exception thrown by the service if the SP returns no record (404)
                return NotFound($"Patient form with ID {formId} was not found or is archived.");
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while retrieving the patient form details."
                );
            }
        }
    }
}