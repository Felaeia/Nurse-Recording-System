namespace NurseRecordingSystem.DTO.AdminServiceDTOs.AdminClinicStatusDTOs
{
    public class CreateClinicStatusRequestDTO
    {
        public bool Status { get; set; }
        public int NurseId { get; set; } // The nurse responsible for logging the status
    }
}
