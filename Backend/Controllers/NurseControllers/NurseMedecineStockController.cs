using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseMedecineStock;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseMedecineStockDTOs;

[Route("api/admin/medecine")]
[ApiController]
// [Authorize(Roles = "Admin")] // Authorization should be applied in a real application
public class NurseMedecineStockController : ControllerBase
{
    private readonly ICreateMedecineStock _createService;
    private readonly IDeleteMedecineStock _deleteService;
    private readonly IUpdateMedecineStock _updateService;

    public NurseMedecineStockController(ICreateMedecineStock createService, IDeleteMedecineStock deleteService, IUpdateMedecineStock updateService)
    {
        _createService = createService;
        _deleteService = deleteService;
        _updateService = updateService;
    }

    [HttpPost("create_stock")]
    [Authorize(Policy = "MustBeNurse")]
    public async Task<IActionResult> CreateStock([FromBody] CreateMedecineStockRequestDTO request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Fetching user identity from the controller's context (simulated or real auth)
        var createdById = User?.Identity?.Name ?? "AdminUser";

        try
        {
            var newId = await _createService.CreateMedecineStockAsync(request, createdById);
            return CreatedAtAction(nameof(CreateStock), new { id = newId }, new { MedicineId = newId, Message = "Stock created successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("delete_stock/{medecineStockId}")]
    [Authorize(Policy = "MustBeNurse")]
    public async Task<IActionResult> DeleteStock(int medecineStockId)
    {
        var deletedBy = User?.Identity?.Name ?? "AdminUser";

        try
        {
            var success = await _deleteService.DeleteMedecineStockAsync(medecineStockId, deletedBy);

            if (!success)
            {
                return NotFound($"Medicine Stock with ID {medecineStockId} not found or is inactive.");
            }

            return NoContent(); // HTTP 204 No Content for successful deletion
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("update_stock/{medecineStockId}")]
    [Authorize(Policy = "MustBeNurse")]
    public async Task<IActionResult> UpdateStock(int medecineStockId, [FromBody] UpdateMedecineStockRequestDTO request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updatedBy = User?.Identity?.Name ?? "NurseUser";

        try
        {
            var success = await _updateService.UpdateAsync(medecineStockId, request, updatedBy);

            if (!success)
            {
                return NotFound($"Medicine Stock with ID {medecineStockId} not found or is inactive.");
            }

            return NoContent(); // HTTP 204 No Content for successful update
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}