using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.DTO.NurseServiceDTOs.NursePatientRecordDTOs
{
    // DTO for a single item in the list view (nsp_ViewPatientRecordList)
    public class PatientRecordListItemDTO
    {
        [Required]
        public int PatientRecordId { get; set; }

        [Required]
        [MaxLength(50)]
        public string NursingDiagnosis { get; set; } = string.Empty;

        [Required]
        public string NursingIntervention { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; set; } = string.Empty;
    }
    // DTO for returning a collection of list items
    public class ViewPatientRecordListResponseDTO
    {
        [Required]
        public List<PatientRecordListItemDTO> Records { get; set; } = new List<PatientRecordListItemDTO>();
    }
}
