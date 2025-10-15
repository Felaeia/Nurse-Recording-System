using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UserFormsDTOs;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// DTO for returning a collection of list items (Optional, often the controller returns List<UserFormListItemDTO>).
/// </summary>
public class ViewUserFormListResponseDTO
{
    [Required]
    public List<UserFormListItemDTO> Forms { get; set; } = new List<UserFormListItemDTO>();
}