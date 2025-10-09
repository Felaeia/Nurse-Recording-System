using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.Model.DTO.UserServiceDTOs.UsersDTOs
{
    public class ViewUserProfileDTO
    {
        public int UserId { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        public string? MiddleName { get; set; }

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string ContactNumber { get; set; } = string.Empty;

        public string? Address { get; set; }

        public int Role { get; set; }
    }
}