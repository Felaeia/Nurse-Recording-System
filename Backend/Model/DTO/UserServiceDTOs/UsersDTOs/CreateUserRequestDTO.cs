using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.Model.DTO.UserServiceDTOs.UsersDTOs
{
    public class CreateUserRequestDTO
    {
        [Required]
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        
        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public string ContactNumber { get; set; } = null!;

        public int AuthId { get; set; }
    }
}
