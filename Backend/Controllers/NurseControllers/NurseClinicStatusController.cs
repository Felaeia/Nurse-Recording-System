using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseClinicStatus;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseClinicStatusDTOs;


[Route("api/nurse/clinicstatus")]
[ApiController]
public class NurseClinicStatusController : ControllerBase
{
    private readonly IUpdateClinicStatus _updateService;

    public NurseClinicStatusController(IUpdateClinicStatus updateService)
    {
        _updateService = updateService;
    }

    [HttpPut("update_clinic_status/{id}")]
    [Authorize(Policy = "MustBeNurse")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateClinicStatusRequestDTO request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updatedBy = User?.Identity?.Name ?? "NurseSystem";

        try
        {
            var success = await _updateService.UpdateAsync(id, request, updatedBy);

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