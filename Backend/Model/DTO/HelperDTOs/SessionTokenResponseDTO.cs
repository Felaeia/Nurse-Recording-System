namespace NurseRecordingSystem.Model.DTO.HelperDTOs
{
    public class SessionTokenResponseDto
    {
        public byte[] Token { get; set; } = null!;
        public int UserId { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsActive { get; set; }
    }
}
