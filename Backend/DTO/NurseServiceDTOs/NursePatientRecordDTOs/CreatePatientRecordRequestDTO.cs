using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.DTO.NurseServiceDTOs.NursePatientRecordDTOs
{
    // DTO for creating a new patient record (nsp_CreatePatientRecord request)
    public class CreatePatientRecordRequestDTO
    {
        [Required]
        [MaxLength(50)]
        public string NursingDiagnosis { get; set; } = string.Empty;

        [Required]
        public string NursingIntervention { get; set; } = string.Empty;

        [Required]
        public int NurseId { get; set; }

        // Note: createdBy will typically come from the controller/token, not the request body.
    }
}
