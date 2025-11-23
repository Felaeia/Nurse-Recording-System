namespace NurseRecordingSystem.DTO.NurseServiceDTOs.NurseAppointmentScheduleDTOs
{
    public class ViewAppointmentScheduleListResponseDTO
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string PatientName { get; set; } = null!;
        public string AppointmentDescription { get; set; } = null!;
    }
}
