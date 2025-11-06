using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ServiceContracts.IAdminServices.IAdminPatientRecords;

namespace NurseRecordingSystem.API.Controllers
{
    // Controller for administrators to perform deletion/archival
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "Admin, SuperAdmin")]
    public class AdminPatientRecordController : ControllerBase
    {
        private readonly IDeletedPatientRecord _deleteService;

        public AdminPatientRecordController(IDeletedPatientRecord deleteService)
        {
            _deleteService = deleteService;
        }

        // DELETE: api/AdminPatientRecord/{patientRecordId}
        [HttpDelete("delete/patient_record/{patientRecordId}")]
        [Authorize(Policy = "MustBeNurse")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteRecord(int patientRecordId)
        {
            try
            {
                bool success = await _deleteService.SoftDeletePatientRecordAsync(patientRecordId);

                if (success)
                {
                    return NoContent(); // 204 success, no body returned
                }
                // If soft delete returns false, the record was already archived/not found.
                return NotFound($"Patient record with ID {patientRecordId} was not found or is already deleted.");
            }
            catch (KeyNotFoundException ex)
            {
                // Catches the specific not found error from the service
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }
    }
}