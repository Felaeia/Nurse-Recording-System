namespace NurseRecordingSystem.Model.DTO.AuthDTOs
{
    public class LoginResponseDTO
    {
        public int AuthId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Role { get; set; }
        public bool IsAuthenticated { get; set; }

        //public int Token { get; set; } //JWT Token or Session Token

    }
}
