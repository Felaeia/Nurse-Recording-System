using System.ComponentModel.DataAnnotations;

/// <summary>
/// DTO for the comprehensive view of a single patient form (as required by hsp_ViewUserForm).
/// </summary>
public class ViewUserFormResponseDTO
{
    [Required]
    public int FormId { get; set; }

    [Required]
    public string IssueType { get; set; } = string.Empty;

    public string? IssueDescryption { get; set; }

    [Required]
    [MaxLength(10)]
    public string Status { get; set; } = string.Empty;

    [Required]
    public int UserId { get; set; }

    [Required]
    public string PatientName { get; set; } = string.Empty;

    // Audit Fields
    public DateTime? CreatedOn { get; set; }

    [MaxLength(50)]
    public string? CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    [MaxLength(50)]
    public string? UpdatedBy { get; set; }

    [Required]
    public bool IsActive { get; set; }
}