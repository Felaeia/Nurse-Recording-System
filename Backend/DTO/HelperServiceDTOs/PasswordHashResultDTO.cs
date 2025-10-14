namespace NurseRecordingSystem.DTO.HelperServiceDTOs
{
    public class PasswordHashResultDTO
    {
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;

    }
}
