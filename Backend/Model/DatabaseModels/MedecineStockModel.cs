namespace NurseRecordingSystem.Model.DatabaseModels
{
    public class MedecineStockModel
    {
        public int medicineId { get; set; }
        public string medecineName { get; set; } = null!;
        public string medecineDescription { get; set; } = null!;
        public int numberOfStock { get; set; }
        public int nurseId { get; set; }

    }
}
