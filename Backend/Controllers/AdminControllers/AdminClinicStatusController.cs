using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ServiceContracts.IAdminServices.IAdminClinicStatus;
using NurseRecordingSystem.DTO.AdminServiceDTOs.AdminClinicStatusDTOs;

[Route("api/admin/clinicstatus")]
[ApiController]
public class AdminClinicStatusController : ControllerBase
{
    private readonly ICreateClinicStatus _createService;
    private readonly IDeleteClinicStatus _deleteService;

    public AdminClinicStatusController(ICreateClinicStatus createService, IDeleteClinicStatus deleteService)
    {
        _createService = createService;
        _deleteService = deleteService;
    }

    [HttpPost("create/clinic_status")]
    public async Task<IActionResult> CreateStatus([FromBody] CreateClinicStatusRequestDTO request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var createdBy = User?.Identity?.Name ?? "AdminSystem";

        try
        {
            var newId = await _createService.CreateAsync(request, createdBy);
            return CreatedAtAction(nameof(CreateStatus), new { id = newId }, new { LogId = newId, Message = "Clinic status log created." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("delete/clinic_status/{id}")]
    public async Task<IActionResult> DeleteStatus(int id)
    {
        var deletedBy = User?.Identity?.Name ?? "AdminSystem";

        try
        {
            var success = await _deleteService.DeleteAsync(id, deletedBy);

            if (!success)
            {
                return NotFound($"Clinic Status Log with ID {id} not found or is inactive.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}