using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.DTO.NurseServiceDTOs.NurseAppointmentScheduleDTOs
{
    public class DeleteAppointmentScheduleRequestDTO
    {
        [Required]
        public int AppointmentId { get; set; }

        // ID of the nurse performing the delete (for validation)
        [Required]
        public int NurseId { get; set; }

        // User name of the deleter
        [Required]
        [MaxLength(50)]
        public string DeletedBy { get; set; } = null!;
    }
}
