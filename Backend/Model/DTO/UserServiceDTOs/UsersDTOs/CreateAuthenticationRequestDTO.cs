using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.Model.DTO.UserServiceDTOs.UsersDTOs
{
    public class CreateAuthenticationRequestDTO
    {
        [Required]
        public string UserName { get; set; } = null!;
        
        [Required]
        public string Password { get; set; } = null!;
        
        [EmailAddress]
        [Required]
        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }

        public string Address { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string ContactNumber { get; set; } = null!;

        public int AuthId { get; set; }

    }
}
