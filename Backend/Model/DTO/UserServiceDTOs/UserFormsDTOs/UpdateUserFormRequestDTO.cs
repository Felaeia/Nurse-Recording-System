using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.Model.DTO.UserServiceDTOs.UserFormsDTOs
{
    public class UpdateUserFormRequestDTO
    {
        [Required]
        public int formId { get; set; }

        [Required]
        public string issueType { get; set; } = null!;
        public string? issueDescryption { get; set; }

        [Required]
        public string status { get; set; } = null!;

        [Required]
        public string patientName { get; set; } = null!;
    }
}
