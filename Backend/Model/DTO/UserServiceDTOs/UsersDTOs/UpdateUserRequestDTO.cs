// File: NurseRecordingSystem.Model.DTO.UserDTOs/UpdateUserRequestDTO.cs

using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.Model.DTO.UserDTOs
{
    public class UpdateUserRequestDTO
    {
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; } = string.Empty;

        // Allows null value for MiddleName
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact Number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string ContactNumber { get; set; } = string.Empty;

        // Allows null value for Address
        public string? Address { get; set; }
    }
}