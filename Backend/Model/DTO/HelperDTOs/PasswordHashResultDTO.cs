namespace NurseRecordingSystem.Model.DTO.HelperDTOs
{
    public class PasswordHashResultDTO
    {
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;

    }
}
