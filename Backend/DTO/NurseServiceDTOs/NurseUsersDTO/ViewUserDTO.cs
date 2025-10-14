using System;
using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.Model.DTO.UserServiceDTOs.UsersDTOs
{
    /// <summary>
    /// DTO representing the comprehensive profile of a single user, 
    /// combining data from the Users and Auth tables.
    /// </summary>
    public class ViewUserDTO
    {
        // Primary Key
        [Required]
        public int UserId { get; set; }

        // Auth/Login Fields
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = string.Empty;

        // Personal Profile Fields
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? MiddleName { get; set; } // Nullable field

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(15)]
        public string ContactNumber { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Address { get; set; } // Nullable field

        // Status Field
        [Required]
        public bool IsActive { get; set; }

        // Audit Fields
        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; set; } = string.Empty;

        [Required]
        public DateTime UpdatedOn { get; set; }

        [Required]
        [MaxLength(50)]
        public string UpdatedBy { get; set; } = string.Empty;
    }

    // Optional container DTO for API responses, though the controller typically returns List<ViewUserDTO> directly.
    public class ViewAllUsersDTO
    {
        [Required]
        public List<ViewUserDTO> Users { get; set; } = new List<ViewUserDTO>();
    }
}