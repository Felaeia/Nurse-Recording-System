using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.DTO.NurseServiceDTOs.NurseClinicStatusDTOs
{
    public class UpdateClinicStatusRequestDTO
    {
        [Required]
        public bool Status { get; set; } // true for Open, false for Closed
    }
}
