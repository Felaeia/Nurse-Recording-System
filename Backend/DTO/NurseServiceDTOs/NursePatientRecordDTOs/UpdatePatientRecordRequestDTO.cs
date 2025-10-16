using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.DTO.NurseServiceDTOs.NursePatientRecordDTOs
{
    // DTO for updating an existing patient record (nsp_UpdatePatientRecord request)
    public class UpdatePatientRecordRequestDTO
    {
        [Required]
        public int PatientRecordId { get; set; }

        [Required]
        [MaxLength(50)]
        public string NursingDiagnosis { get; set; } = string.Empty;

        [Required]
        public string NursingIntervention { get; set; } = string.Empty;
    }
}
