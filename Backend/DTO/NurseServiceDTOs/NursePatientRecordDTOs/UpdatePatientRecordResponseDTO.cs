namespace NurseRecordingSystem.DTO.NurseServiceDTOs.NursePatientRecordDTOs
{
    // DTO for returning the result of an update (often just success confirmation)
    public class UpdatePatientRecordResponseDTO
    {
        public bool Success { get; set; }
        public int UpdatedRecordId { get; set; }
    }
}
