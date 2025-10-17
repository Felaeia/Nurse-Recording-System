using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.Model.DTO.NurseServicesDTOs.NurseCreation
{
    /// <summary>
    /// Request DTO for creating a new nurse account (Auth + Profile).
    /// </summary>
    public class CreateNurseRequestDTO
    {
        // Auth fields
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = null!;

        [Required]
        [MinLength(8)] // Assuming a minimum length for security
        public string Password { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = null!;

        // Nurse profile fields
        [Required]
        public string FirstName { get; set; } = null!;

        [MaxLength(5)]
        public string? MiddleName { get; set; }

        [Required]
        public string LastName { get; set; } = null!;

        [MaxLength(11)]
        public string? ContactNumber { get; set; }
    }
}