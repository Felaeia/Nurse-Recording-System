namespace NurseRecordingSystem.DTO.AuthServiceDTOs
{
    public class UserDetailsDTO
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string? Address { get; set; }
    }
}
