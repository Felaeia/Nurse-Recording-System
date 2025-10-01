namespace NurseRecordingSystem.Model.DatabaseModels
{
    public class NursesModel
    {
        public int nurseId { get; set; }
        public int authId { get; set; }
        public string firstName { get; set; } = null!;
        public string middleName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string contactNumber { get; set; } = null!;

    }
}
