namespace NurseRecordingSystem.Model.DatabaseModels
{
    public class UserModel
    {
        public int UserId { get; set; }
        public int AuthId { get; set; }
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public string? Address { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }

    }
}
