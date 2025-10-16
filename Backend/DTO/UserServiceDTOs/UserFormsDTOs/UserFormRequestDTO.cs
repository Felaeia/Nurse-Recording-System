using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.DTO.UserServiceDTOs.UserFormsDTOs
{
    public class UserFormRequestDTO
    {
        [Required]
        public string issueType { get; set; } = null!;
        public string? issueDescryption { get; set; }

        [Required]
        public string status { get; set; } = null!;

        [Required]
        public string patientName { get; set; } = null!;

        [Required]
        public string createdBy { get; set; } = null!;

        [Required]
        public string updatedBy { get; set; } = null!;

        [Required]
        public string DeletedBy { get; set; } = null!;

    }
}
