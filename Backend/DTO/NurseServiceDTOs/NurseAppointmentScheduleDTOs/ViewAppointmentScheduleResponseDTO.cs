namespace NurseRecordingSystem.DTO.NurseServiceDTOs.NurseAppointmentScheduleDTOs
{
    public class ViewAppointmentScheduleResponseDTO
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string AppointmentDescription { get; set; } = null!;
        public int NurseId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
