using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.NurseCreation;
using NurseRecordingSystem.Model.DTO.NurseServicesDTOs.NurseCreation;
using System;

namespace NurseRecordingSystem.API.Controllers
{
    // AdminNuresController.cs
    [Route("api/admin/nurse")]
    [ApiController]
    // [Authorize(Roles = "Admin, SuperAdmin")] 
    public class AdminNurseController : ControllerBase
    {
        private readonly ICreateNurse _createNurseService;

        public AdminNurseController(ICreateNurse createNurseService)
        {
            _createNurseService = createNurseService;
        }

        // POST: api/admin/nurse/register
        /// <summary>
        /// Creates a new Nurse account (Auth and Profile) with the role 'Nurse'.
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)] // For existing email
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterNurse([FromBody] CreateNurseRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // The service returns the new AuthId
                int newAuthId = await _createNurseService.CreateNurseAsync(request);

                return CreatedAtAction(
                    nameof(RegisterNurse),
                    new { authId = newAuthId },
                    new { AuthId = newAuthId, Message = "Nurse account created successfully with role 'Nurse'." }
                );
            }
            catch (InvalidOperationException ex)
            {
                // Catches the specific SP error regarding existing email (50005)
                return Conflict(new { Error = ex.Message }); // 409 Conflict
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { Error = "Account creation failed due to internal error.", Detail = ex.Message }
                );
            }
        }
    }
}