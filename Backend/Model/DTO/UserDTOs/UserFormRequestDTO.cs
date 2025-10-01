namespace NurseRecordingSystem.Model.DTO.UserDTOs
{
    public class UserFormRequestDTO
    {
        public int formId { get; set; }
        public string issueType { get; set; } = null!;
        public string? issueDescryption { get; set; }
        public string status { get; set; } = null!;
        public int userId { get; set; }
        public string patientName { get; set; } = null!;
        public string createdBy { get; set; } = null!;
        public string updatedBy { get; set; } = null!;
        public string DeletedBy { get; set; } = null!;
        public byte[] MyProperty1 { get; set; } = null!;


    }
}
