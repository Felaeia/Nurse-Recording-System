namespace NurseRecordingSystem.DTO.AuthServiceDTOs
{
    public class LoginResponseDTO
    {
        public int AuthId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool IsAuthenticated { get; set; }

        public UserDetailsDTO? UserDetails { get; set; }
        public NurseDetailsDTO? NurseDetails { get; set; }
    }
}
