namespace NurseRecordingSystem.DTO.NurseServiceDTOs.NurseMedecineStockDTOs
{
    public class CreateMedecineStockRequestDTO
    {
        public string MedecineName { get; set; } = string.Empty;
        public string MedecineDescription { get; set; } = string.Empty;
        public int NumberOfStock { get; set; }
        public int NurseId { get; set; } // The ID of the nurse creating the stock
    }
}
