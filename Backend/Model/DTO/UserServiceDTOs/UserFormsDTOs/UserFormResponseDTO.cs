namespace NurseRecordingSystem.Model.DTO.UserServiceDTOs.UserFormsDTOs
{
    public class UserFormResponseDTO
    {
        public bool IsSuccess { get; set; } 
        public int UserFormId { get; set; }
        public string Message { get; set; } = null!;

    }
}
