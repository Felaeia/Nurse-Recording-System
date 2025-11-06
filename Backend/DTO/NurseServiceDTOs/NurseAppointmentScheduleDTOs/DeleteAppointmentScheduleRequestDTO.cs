using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.DTO.NurseServiceDTOs.NurseAppointmentScheduleDTOs
{
    public class DeleteAppointmentScheduleRequestDTO
    {
        // User name of the deleter
        [Required]
        public int DeletedByNurseId { get; set; }
    }
}
