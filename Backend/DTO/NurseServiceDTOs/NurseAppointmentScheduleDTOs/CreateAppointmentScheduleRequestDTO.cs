using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.DTO.NurseServiceDTOs.NurseAppointmentScheduleDTOs
{
    public class CreateAppointmentScheduleRequestDTO
    {
        // The time of the new appointment
        [Required]
        public DateTime AppointmentTime { get; set; }

        // Description of the appointment
        [Required]
        public string AppointmentDescription { get; set; } = null!;

        // ID of the nurse creating the appointment (used for validation and FK)
        [Required]
        public int NurseId { get; set; }

        // User name of the creator (e.g., from a JWT/Claims)
        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; set; } = null!;
    }
}
