namespace NurseRecordingSystem.Model.DatabaseModels
{
    public class PatientFormModel
    {
        public int formId { get; set; }
        public string issueType { get; set; } = null!;
        public string issueDescryption { get; set; } = null!;
        public string status { get; set; } = null!;
        public int userId { get; set; }

    }
}
