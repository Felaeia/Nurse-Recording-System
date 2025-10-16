namespace NurseRecordingSystem.DTO.AuthServiceDTOs
{
    public class UserAuthDTO
    {
        public int AuthId { get; init; }
        public string? UserName { get; init; } = null!;
        public byte[]? PasswordHash { get; init; } = null!;
        public byte[]? PasswordSalt { get; init; } = null!;
        public string? Email { get; init; } = null!;
        public int Role { get; init; }
    }
}
