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

    }
}
