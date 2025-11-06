namespace NurseRecordingSystem.DTO.AuthServiceDTOs
{
    public class SessionValidationResult
    {
        public bool IsValid { get; set; }
        public byte Token { get; set; }
    }
}
