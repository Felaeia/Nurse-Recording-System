using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.DTO.AdminServiceDTOs.AdminClinicStatusDTOs
{
    public class CreateClinicStatusRequestDTO
    {
        [Required]
        public bool Status { get; set; }
        [Required]
        public int NurseId { get; set; } // The nurse responsible for logging the status
    }
}
