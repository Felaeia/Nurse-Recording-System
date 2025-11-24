using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.DTO.NurseServiceDTOs.NurseAppointmentScheduleDTOs
{
    public class UpdateAppointmentScheduleRequestDTO
    {
        // New time for the appointment
        [Required]
        public DateTime AppointmentTime { get; set; }

        [Required]
        public string PatientName { get; set; } = null!;

        // New description for the appointment
        [Required]
        public string AppointmentDescription { get; set; } = null!;

        // ID of the nurse updating the appointment (used for validation)
        [Required]
        public int NurseId { get; set; }

        // User name of the updater
        [Required]
        [MaxLength(50)]
        public string UpdatedBy { get; set; } = null!;
    }
}
