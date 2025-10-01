namespace NurseRecordingSystem.Model.DTO.AuthDTOs
{
    public class CreateAuthenticationRequestDTO
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;

    }
}
