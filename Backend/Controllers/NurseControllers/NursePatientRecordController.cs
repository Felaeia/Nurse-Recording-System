using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INursePatientRecords;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NursePatientRecordDTOs;
using NurseRecordingSystem.Model.DTO.NurseServicesDTOs.PatientRecordsDTOs;

namespace NurseRecordingSystem.API.Controllers
{
    // Controller for nurses to perform standard record operations
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "Nurse, Admin")] 
    public class NursePatientRecordController : ControllerBase
    {
        private readonly IViewPatientRecord _viewService;
        private readonly IViewPatientRecordList _viewListService;
        private readonly IUpdatePatientRecord _updateService;
        private readonly ICreatePatientRecord _createService;

        public NursePatientRecordController(
            IViewPatientRecord viewService,
            IViewPatientRecordList viewListService,
            IUpdatePatientRecord updateService,
            ICreatePatientRecord createService)
        {
            _viewService = viewService;
            _viewListService = viewListService;
            _updateService = updateService;
            _createService = createService;
        }

        // POST: api/NursePatientRecord
        [HttpPost("create/patient_record")]
        [Authorize(Policy = "MustBeNurse")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRecord([FromBody] CreatePatientRecordRequestDTO request, [FromHeader(Name = "X-Created-By")] string createdBy)
        {
            if (string.IsNullOrEmpty(createdBy)) return BadRequest("Audit header 'X-Created-By' is required.");

            try
            {
                int newId = await _createService.CreatePatientRecordAsync(request, createdBy);
                return CreatedAtAction(nameof(GetRecord), new { patientRecordId = newId }, newId);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // GET: api/NursePatientRecord/{patientRecordId}
        [HttpGet("view/patient_record/{patientRecordId}")]
        [Authorize(Policy = "MustBeNurse")]
        [ProducesResponseType(typeof(ViewPatientRecordResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRecord(int patientRecordId)
        {
            try
            {
                var record = await _viewService.GetPatientRecordAsync(patientRecordId);
                return Ok(record);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }

        // GET: api/NursePatientRecord/list?nurseId=1
        [HttpGet("view/patient_record_list")]
        [Authorize(Policy = "MustBeNurse")]
        [ProducesResponseType(typeof(List<PatientRecordListItemDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecordList([FromQuery] int? nurseId)
        {
            try
            {
                var records = await _viewListService.GetPatientRecordListAsync(nurseId);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }

        // PUT: api/NursePatientRecord/{patientRecordId}
        [HttpPut("update/patient_record/{patientRecordId}")]
        [Authorize(Policy = "MustBeNurse")]
        [ProducesResponseType(typeof(UpdatePatientRecordResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRecord(int patientRecordId, [FromBody] UpdatePatientRecordRequestDTO request, [FromHeader(Name = "X-Updated-By")] string updatedBy)
        {
            if (patientRecordId != request.PatientRecordId)
            {
                return BadRequest("ID mismatch between route and body.");
            }
            if (string.IsNullOrEmpty(updatedBy)) return BadRequest("Audit header 'X-Updated-By' is required.");

            try
            {
                var response = await _updateService.UpdatePatientRecordAsync(request, updatedBy);
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Record with ID {patientRecordId} not found or cannot be updated.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }
    }
}