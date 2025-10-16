using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.Model.DTO.UserServiceDTOs.UserFormsDTOs
{
    /// <summary>
    /// DTO for a single item in the list view (as required by nsp_ViewUserFormList).
    /// </summary>
    public class UserFormListItemDTO
    {
        [Required]
        public int FormId { get; set; }

        [Required]
        public string IssueType { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string Status { get; set; } = string.Empty;

        [Required]
        public string PatientName { get; set; } = string.Empty;
    }
}