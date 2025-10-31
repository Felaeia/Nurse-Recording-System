namespace NurseRecordingSystem.DTO.AuthServiceDTOs
{
    public class SessionTokenDTO
    {
        public int TokenId { get; set; }
        public byte[] Token { get; set; } = null!;
        public int AuthId { get; set; }
        public DateTime ExpiresOn { get; set; }
        public int IsActive { get; set; }
    }
}