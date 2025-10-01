namespace NurseRecordingSystem.Model.DatabaseModels
{
    public class PatientRecordModel
    {
        public int patientRecordId { get; set; }
        public string nursingDiagnosis { get; set; } = null!;
        public string nursingIntervention { get; set; } = null!;
        public int nurseId { get; set; }

    }
}
