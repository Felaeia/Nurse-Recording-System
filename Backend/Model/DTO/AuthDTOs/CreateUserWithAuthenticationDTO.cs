namespace NurseRecordingSystem.Model.DTO.AuthDTOs
{
    public class CreateUserWithAuthenticationDTO
    {
        // Authentication properties
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;

        // User properties
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;

    }
}
