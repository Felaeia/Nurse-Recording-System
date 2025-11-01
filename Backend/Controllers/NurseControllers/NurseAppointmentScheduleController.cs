using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseAppointmentScheduleDTOs;

namespace NurseRecordingSystem.Controllers.NurseControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NurseAppointmentScheduleController : ControllerBase
    {
        private readonly ICreateAppointmentSchedule _createService;
        private readonly IViewAppointmentScheduleList _viewListService;
        private readonly IViewAppointmentSchedule _viewService;
        private readonly IUpdateAppointmentSchedule _updateService;
        private readonly IDeleteAppointmentSchedule _deleteService;

        public NurseAppointmentScheduleController(
            ICreateAppointmentSchedule createService,
            IViewAppointmentScheduleList viewListService,
            IViewAppointmentSchedule viewService,
            IUpdateAppointmentSchedule updateService,
            IDeleteAppointmentSchedule deleteService)
        {
            _createService = createService;
            _viewListService = viewListService;
            _viewService = viewService;
            _updateService = updateService;
            _deleteService = deleteService;
        }

        // POST: api/AppointmentSchedule
        // Create an appointment (Nurse Only)
        [HttpPost("create_appointment")]
        [Authorize(Policy = "MustBeNurse")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentScheduleRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool success = await _createService.CreateAppointmentAsync(request);
                if (success)
                {
                    return StatusCode(201, new { Message = "Appointment created successfully." });
                }
                // Should not be reached if the service throws on explicit error codes (1, 2, -1)
                return StatusCode(500, new { Message = "Failed to create appointment for an unknown reason." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message); // 403 Forbidden
            }
            catch (Exception ex)
            {
                // Log the exception (ex.InnerException if applicable)
                return StatusCode(500, new { Message = "Internal server error.", Detail = ex.Message });
            }
        }

        // GET: api/AppointmentSchedule
        // Get list of all active appointments
        [HttpGet("view_appointment_list")]
        [Authorize(Policy = "MustBeNurse")]
        public async Task<IActionResult> ViewAppointmentScheduleList()
        {
            try
            {
                var appointments = await _viewListService.ViewAppointmentScheduleListAsync();
                return Ok(appointments); // 200 OK
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Message = "Internal server error.", Detail = ex.Message });
            }
        }

        // GET: api/AppointmentSchedule/{id}
        // Get single appointment by ID
        [HttpGet("view_appointment/{id}")]
        [Authorize(Policy = "MustBeNurse")]
        public async Task<IActionResult> ViewAppointmentSchedule(int id)
        {
            try
            {
                var appointment = await _viewService.ViewAppointmentScheduleAsync(id);
                return Ok(appointment); // 200 OK
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404 Not Found
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Message = "Internal server error.", Detail = ex.Message });
            }
        }

        // PUT: api/AppointmentSchedule
        // Update an appointment (Nurse Only)
        [HttpPut("update_appointment/{id}")]
        [Authorize(Policy = "MustBeNurse")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] UpdateAppointmentScheduleRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool success = await _updateService.UpdateAppointmentAsync(id, request);
                if (success)
                {
                    return Ok(new { Message = "Appointment updated successfully." });
                }
                return StatusCode(500, new { Message = "Failed to update appointment for an unknown reason." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message); // 403 Forbidden
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404 Not Found
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Message = "Internal server error.", Detail = ex.Message });
            }
        }

        // DELETE: api/AppointmentSchedule
        // Soft delete an appointment (Nurse Only)
        // Using HTTP DELETE with a body is non-standard but common for soft-deletes needing user context.
        [HttpDelete("delete_appointment/{id}")]
        [Authorize(Policy = "MustBeNurse")]
        public async Task<IActionResult> DeleteAppointment(int id, [FromBody] DeleteAppointmentScheduleRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool success = await _deleteService.DeleteAppointmentAsync(id, request);
                if (success)
                {
                    return Ok(new { Message = "Appointment deleted successfully (soft-deleted)." });
                }
                return StatusCode(500, new { Message = "Failed to delete appointment for an unknown reason." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message); // 403 Forbidden
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404 Not Found
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Message = "Internal server error.", Detail = ex.Message });
            }
        }
    }
}
